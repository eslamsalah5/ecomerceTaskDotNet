using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<Cart?> GetByUserIdWithItemsAsync(string userId);
        Task<Cart> GetOrCreateForUserAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task AddItemAsync(CartItem item);
        Task UpdateItemAsync(CartItem item);
        Task RemoveItemAsync(CartItem item);
        Task RemoveAllItemsAsync(int cartId);
    }
}
