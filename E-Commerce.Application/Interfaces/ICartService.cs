using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetCartAsync(string userId);
        Task<ApiResponse<CartDto>> AddItemAsync(string userId, AddCartItemDto dto);
        Task<ApiResponse<CartDto>> UpdateItemQuantityAsync(string userId, int productId, UpdateCartItemDto dto);
        Task<ApiResponse<CartDto>> IncrementItemQuantityAsync(string userId, int productId);
        Task<ApiResponse<CartDto>> DecrementItemQuantityAsync(string userId, int productId);
        Task<ApiResponse<bool>> RemoveItemAsync(string userId, int productId);
        Task<ApiResponse<bool>> ClearCartAsync(string userId);
    }
}
