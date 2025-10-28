using AutoMapper;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Mappings;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using Moq;
using Xunit;

namespace E_Commerce.Tests
{
    public class CartService_IncrementDecrementTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CartService _cartService;

        public CartService_IncrementDecrementTests()
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
        public async Task IncrementItemQuantityAsync_IncreasesQuantityByOne()
        {
            // Arrange
            string userId = "user123";
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                ProductCode = "P001",
                Price = 100,
                Stock = 10,
                IsDeleted = false
            };

            var cart = new Cart { Id = 1, UserId = userId };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cart.Id,
                Cart = cart,
                ProductId = productId,
                Product = product,
                Quantity = 3,
                UnitPriceAtAdd = 100,
                DiscountPercentageAtAdd = 0
            };

            cart.Items = new List<CartItem> { cartItem };

            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(cart.Id, productId)).ReturnsAsync(cartItem);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _imageServiceMock.Setup(s => s.GetImageUrl(It.IsAny<string>())).Returns("/uploads/test.jpg");
            _cartRepositoryMock.Setup(r => r.UpdateItemAsync(cartItem)).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(cart)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cartService.IncrementItemQuantityAsync(userId, productId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(4, cartItem.Quantity); // Incremented from 3 to 4
        }

        [Fact]
        public async Task IncrementItemQuantityAsync_ReturnsError_WhenStockExceeded()
        {
            // Arrange
            string userId = "user123";
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                ProductCode = "P001",
                Price = 100,
                Stock = 5, // Current quantity in cart is 5, stock is 5
                IsDeleted = false
            };

            var cart = new Cart { Id = 1, UserId = userId };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cart.Id,
                Cart = cart,
                ProductId = productId,
                Product = product,
                Quantity = 5, // Already at maximum stock
                UnitPriceAtAdd = 100,
                DiscountPercentageAtAdd = 0
            };

            cart.Items = new List<CartItem> { cartItem };

            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(cart.Id, productId)).ReturnsAsync(cartItem);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _cartService.IncrementItemQuantityAsync(userId, productId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Cannot increment quantity", result.Message);
        }

        [Fact]
        public async Task DecrementItemQuantityAsync_DecreasesQuantityByOne()
        {
            // Arrange
            string userId = "user123";
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                ProductCode = "P001",
                Price = 100,
                Stock = 10,
                IsDeleted = false
            };

            var cart = new Cart { Id = 1, UserId = userId };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cart.Id,
                Cart = cart,
                ProductId = productId,
                Product = product,
                Quantity = 3,
                UnitPriceAtAdd = 100,
                DiscountPercentageAtAdd = 0
            };

            cart.Items = new List<CartItem> { cartItem };

            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(cart.Id, productId)).ReturnsAsync(cartItem);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _imageServiceMock.Setup(s => s.GetImageUrl(It.IsAny<string>())).Returns("/uploads/test.jpg");
            _cartRepositoryMock.Setup(r => r.UpdateItemAsync(cartItem)).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(cart)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cartService.DecrementItemQuantityAsync(userId, productId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(2, cartItem.Quantity); // Decremented from 3 to 2
        }

        [Fact]
        public async Task DecrementItemQuantityAsync_RemovesItem_WhenQuantityIsOne()
        {
            // Arrange
            string userId = "user123";
            int productId = 1;

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                ProductCode = "P001",
                Price = 100,
                Stock = 10,
                IsDeleted = false
            };

            var cart = new Cart { Id = 1, UserId = userId };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cart.Id,
                Cart = cart,
                ProductId = productId,
                Product = product,
                Quantity = 1, // Last item
                UnitPriceAtAdd = 100,
                DiscountPercentageAtAdd = 0
            };

            cart.Items = new List<CartItem> { cartItem };

            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId)).ReturnsAsync(cart);
            _cartRepositoryMock.Setup(r => r.GetCartItemAsync(cart.Id, productId)).ReturnsAsync(cartItem);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _imageServiceMock.Setup(s => s.GetImageUrl(It.IsAny<string>())).Returns("/uploads/test.jpg");
            _cartRepositoryMock.Setup(r => r.RemoveItemAsync(cartItem)).Returns(Task.CompletedTask);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(cart)).ReturnsAsync(cart);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // After removal, return empty cart
            _cartRepositoryMock.Setup(r => r.GetByUserIdWithItemsAsync(userId))
                .ReturnsAsync(new Cart { Id = 1, UserId = userId, Items = new List<CartItem>() });

            // Act
            var result = await _cartService.DecrementItemQuantityAsync(userId, productId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            _cartRepositoryMock.Verify(r => r.RemoveItemAsync(cartItem), Times.Once);
        }
    }
}
