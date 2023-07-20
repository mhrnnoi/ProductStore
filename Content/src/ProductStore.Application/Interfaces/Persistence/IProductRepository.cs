using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Interfaces.Persistence;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetByEmailAsync(string email);
    Task<Product?> GetByDateAsync(DateTime date);

}
