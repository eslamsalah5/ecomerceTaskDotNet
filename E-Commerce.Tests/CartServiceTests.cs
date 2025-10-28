using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Mappings;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using Moq;
using Xunit;

namespace E_Commerce.Tests
{
    public class CartServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _imageServiceMock = new Mock<IImageService>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _unitOfWorkMock.Setup(u => u.Carts).Returns(_cartRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<CartMappingProfile>();
            });
            _mapper = config.CreateMapper();

            _cartService = new CartService(_unitOfWorkMock.Object, _imageServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetCartAsync_ReturnsEmptyCart_WhenNoCartExists()
        {
            // Arrange
            string userId = "user123";
            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync((Cart?)null);

            // Act
            var result = await _cartService.GetCartAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data.Items);
        }

        [Fact]
        public async Task AddItemAsync_ReturnsError_WhenProductNotFound()
        {
            // Arrange
            string userId = "user123";
            var dto = new AddCartItemDto { ProductId = 999, Quantity = 2 };
            _productRepositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _cartService.AddItemAsync(userId, dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("not found", result.Message);
        }

        [Fact]
        public async Task AddItemAsync_ReturnsError_WhenInsufficientStock()
        {
            // Arrange
            string userId = "user123";
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                ProductCode = "P01",
                Category = "Test",
                Price = 100,
                Stock = 3  // Only 3 in stock
            };
            var dto = new AddCartItemDto { ProductId = 1, Quantity = 5 };  // Try to add 5

            _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _cartService.AddItemAsync(userId, dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Insufficient stock", result.Message);
        }

        [Fact]
        public async Task AddItemAsync_AddsNewItem_WhenValid()
        {
            // Arrange
            string userId = "user123";
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                ProductCode = "P01",
                Category = "Test",
                Price = 100,
                Stock = 50,
                DiscountPercentage = 10
            };
            var cart = new Cart
            {
                Id = 1,
                UserId = userId,
                Items = new List<CartItem>()
            };
            var dto = new AddCartItemDto { ProductId = 1, Quantity = 2 };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _cartRepositoryMock.Setup(r => r.GetOrCreateForUserAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(1, 1)).ReturnsAsync((CartItem?)null);
            _cartRepositoryMock.Setup(r => r.AddItemAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Cart>())).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var cartWithItems = new Cart
            {
                Id = 1,
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        CartId = 1,
                        ProductId = 1,
                        Quantity = 2,
                        UnitPriceAtAdd = 100,
                        DiscountPercentageAtAdd = 10,
                        Product = product
                    }
                }
            };
            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cartWithItems);
            _imageServiceMock.Setup(i => i.GetImageUrl(It.IsAny<string>())).Returns("/img.jpg");

            // Act
            var result = await _cartService.AddItemAsync(userId, dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.Items);
        }

        [Fact]
        public async Task RemoveItemAsync_ReturnsError_WhenCartNotFound()
        {
            // Arrange
            string userId = "user123";
            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync((Cart?)null);

            // Act
            var result = await _cartService.RemoveItemAsync(userId, 1);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Contains("Cart not found", result.Message);
        }

        [Fact]
        public async Task RemoveItemAsync_RemovesItem_WhenExists()
        {
            // Arrange
            string userId = "user123";
            var cart = new Cart { Id = 1, UserId = userId, Items = new List<CartItem>() };
            var cartItem = new CartItem { CartId = 1, ProductId = 1, Quantity = 2 };

            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(1, 1)).ReturnsAsync(cartItem);
            _cartRepositoryMock.Setup(r => r.RemoveItemAsync(cartItem)).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(cart)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cartService.RemoveItemAsync(userId, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ClearCartAsync_ClearsAllItems_WhenCartExists()
        {
            // Arrange
            string userId = "user123";
            var cart = new Cart { Id = 1, UserId = userId };

            _cartRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.RemoveAllItemsAsync(1)).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(cart)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cartService.ClearCartAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Data);
        }
    }
}
