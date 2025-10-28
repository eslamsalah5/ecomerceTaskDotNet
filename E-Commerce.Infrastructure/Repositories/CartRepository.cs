using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            // Global query filter handles IsDeleted check
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetByUserIdWithItemsAsync(string userId)
        {
            // Global query filter handles IsDeleted check
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> GetOrCreateForUserAsync(string userId)
        {
            var cart = await GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Total = 0
                };
                await _dbSet.AddAsync(cart);
                // Removed auto-save - caller must call UnitOfWork.SaveChangesAsync()
            }

            return cart;
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.Set<CartItem>()
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task AddItemAsync(CartItem item)
        {
            await _context.Set<CartItem>().AddAsync(item);
        }

        public Task UpdateItemAsync(CartItem item)
        {
            _context.Set<CartItem>().Update(item);
            return Task.CompletedTask;
        }

        public Task RemoveItemAsync(CartItem item)
        {
            _context.Set<CartItem>().Remove(item);
            return Task.CompletedTask;
        }

        public async Task RemoveAllItemsAsync(int cartId)
        {
            var items = await _context.Set<CartItem>()
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.Set<CartItem>().RemoveRange(items);
        }
    }
}
