using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<PagedResult<ProductDto>>> GetAllProductsAsync(PaginationParameters paginationParameters);
        Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto);
        Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
    }
}