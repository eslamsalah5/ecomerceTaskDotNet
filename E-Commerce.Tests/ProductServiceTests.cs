using E_Commerce.Application.DTOs;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace E_Commerce.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _imageServiceMock = new Mock<IImageService>();
            _productService = new ProductService(_unitOfWorkMock.Object, _imageServiceMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
        {
            var product = new Product { Id = 1, Name = "Test", Category = "Cat", ProductCode = "P01", Price = 10, MinimumQuantity = 1 };
            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(1)).ReturnsAsync(product);
            _imageServiceMock.Setup(i => i.GetImageUrl(It.IsAny<string>())).Returns("/img.jpg");
            var result = await _productService.GetProductByIdAsync(1);
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Test", result.Data.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsError_WhenNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(2)).ReturnsAsync((Product)null);
            var result = await _productService.GetProductByIdAsync(2);
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsSuccess_WhenDeleted()
        {
            _unitOfWorkMock.Setup(u => u.Products.ExistsAsync(1)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Products.SoftDeleteAsync(1)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            var result = await _productService.DeleteProductAsync(1);
            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsError_WhenNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Products.ExistsAsync(2)).ReturnsAsync(false);
            var result = await _productService.DeleteProductAsync(2);
            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
