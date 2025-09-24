using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces
{
    public interface IGenericRepository<T> where T : SoftDeletableEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<PagedResult<T>> GetPagedAsync(PaginationParameters paginationParameters);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}