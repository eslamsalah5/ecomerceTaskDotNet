using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDto>> GetCartAsync(string userId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdWithItemsAsync(userId);

                if (cart == null)
                {
                    return ApiResponse<CartDto>.SuccessResult(new CartDto(), "Cart is empty");
                }

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.SuccessResult(cartDto, "Cart retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CartDto>.ErrorResult($"Error retrieving cart: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<CartDto>> AddItemAsync(string userId, AddCartItemDto dto)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {dto.ProductId} not found", null, 404);
                }

                if (product.Stock < dto.Quantity)
                {
                    return ApiResponse<CartDto>.ErrorResult(
                        $"Insufficient stock. Only {product.Stock} units available for this product",
                        null,
                        400);
                }

                var cart = await _unitOfWork.Carts.GetOrCreateForUserAsync(userId);
                // Save immediately if cart was just created
                await _unitOfWork.SaveChangesAsync();
                
                var existingItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, dto.ProductId);

                if (existingItem != null)
                {
                    var newQuantity = existingItem.Quantity + dto.Quantity;
                    if (newQuantity > product.Stock)
                    {
                        return ApiResponse<CartDto>.ErrorResult(
                            $"Cannot add {dto.Quantity} more units. Only {product.Stock - existingItem.Quantity} units available",
                            null,
                            400);
                    }

                    existingItem.Quantity = newQuantity;
                    existingItem.UnitPriceAtAdd = product.Price;
                    existingItem.DiscountPercentageAtAdd = product.DiscountPercentage;
                    await _unitOfWork.Carts.UpdateItemAsync(existingItem);
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        UnitPriceAtAdd = product.Price,
                        DiscountPercentageAtAdd = product.DiscountPercentage
                    };
                    await _unitOfWork.Carts.AddItemAsync(cartItem);
                }

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                var updatedCart = await _unitOfWork.Carts.GetByUserIdWithItemsAsync(userId);
                var cartDto = _mapper.Map<CartDto>(updatedCart);
                return ApiResponse<CartDto>.SuccessResult(cartDto, "Item added to cart successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CartDto>.ErrorResult($"Error adding item to cart: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<CartDto>> UpdateItemQuantityAsync(string userId, int productId, UpdateCartItemDto dto)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.ErrorResult("Cart not found", null, 404);
                }

                var cartItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, productId);
                if (cartItem == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found in cart", null, 404);
                }

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found", null, 404);
                }

                if (dto.Quantity > product.Stock)
                {
                    return ApiResponse<CartDto>.ErrorResult(
                        $"Insufficient stock. Only {product.Stock} units available",
                        null,
                        400);
                }

                cartItem.Quantity = dto.Quantity;
                cartItem.UnitPriceAtAdd = product.Price;
                cartItem.DiscountPercentageAtAdd = product.DiscountPercentage;
                await _unitOfWork.Carts.UpdateItemAsync(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                var updatedCart = await _unitOfWork.Carts.GetByUserIdWithItemsAsync(userId);
                var cartDto = _mapper.Map<CartDto>(updatedCart);
                return ApiResponse<CartDto>.SuccessResult(cartDto, "Cart item updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CartDto>.ErrorResult($"Error updating cart item: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<CartDto>> IncrementItemQuantityAsync(string userId, int productId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.ErrorResult("Cart not found", null, 404);
                }

                var cartItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, productId);
                if (cartItem == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found in cart", null, 404);
                }

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found", null, 404);
                }

                var newQuantity = cartItem.Quantity + 1;
                if (newQuantity > product.Stock)
                {
                    return ApiResponse<CartDto>.ErrorResult(
                        $"Cannot increment quantity. Only {product.Stock} units available",
                        null,
                        400);
                }

                if (newQuantity > 1000)
                {
                    return ApiResponse<CartDto>.ErrorResult(
                        "Maximum quantity limit (1000) reached",
                        null,
                        400);
                }

                cartItem.Quantity = newQuantity;
                cartItem.UnitPriceAtAdd = product.Price;
                cartItem.DiscountPercentageAtAdd = product.DiscountPercentage;
                await _unitOfWork.Carts.UpdateItemAsync(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                var updatedCart = await _unitOfWork.Carts.GetByUserIdWithItemsAsync(userId);
                var cartDto = _mapper.Map<CartDto>(updatedCart);
                return ApiResponse<CartDto>.SuccessResult(cartDto, "Item quantity incremented successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CartDto>.ErrorResult($"Error incrementing item quantity: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<CartDto>> DecrementItemQuantityAsync(string userId, int productId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.ErrorResult("Cart not found", null, 404);
                }

                var cartItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, productId);
                if (cartItem == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found in cart", null, 404);
                }

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return ApiResponse<CartDto>.ErrorResult($"Product with ID {productId} not found", null, 404);
                }

                var newQuantity = cartItem.Quantity - 1;

                if (newQuantity < 1)
                {
                    await _unitOfWork.Carts.RemoveItemAsync(cartItem);
                }
                else
                {
                    cartItem.Quantity = newQuantity;
                    cartItem.UnitPriceAtAdd = product.Price;
                    cartItem.DiscountPercentageAtAdd = product.DiscountPercentage;
                    await _unitOfWork.Carts.UpdateItemAsync(cartItem);
                }

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                var updatedCart = await _unitOfWork.Carts.GetByUserIdWithItemsAsync(userId);
                var cartDto = _mapper.Map<CartDto>(updatedCart);
                return ApiResponse<CartDto>.SuccessResult(cartDto, "Item quantity decremented successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CartDto>.ErrorResult($"Error decrementing item quantity: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> RemoveItemAsync(string userId, int productId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<bool>.ErrorResult("Cart not found", null, 404);
                }

                var cartItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, productId);
                if (cartItem == null)
                {
                    return ApiResponse<bool>.ErrorResult($"Product with ID {productId} not found in cart", null, 404);
                }

                await _unitOfWork.Carts.RemoveItemAsync(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, "Item removed from cart successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error removing item from cart: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> ClearCartAsync(string userId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<bool>.SuccessResult(true, "Cart is already empty");
                }

                await _unitOfWork.Carts.RemoveAllItemsAsync(cart.Id);

                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, "Cart cleared successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error clearing cart: {ex.Message}", null, 500);
            }
        }
    }
}
