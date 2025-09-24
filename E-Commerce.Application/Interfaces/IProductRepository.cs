using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByProductCodeAsync(string productCode);
        Task<bool> ProductCodeExistsAsync(string productCode, int? excludeId = null);
    }
}