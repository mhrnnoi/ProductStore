using ProductStore.Domain.Products.Entities;

namespace ProductStore.Domain.Abstractions;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<List<Product>> GetUserProductsAsync(string userId);
    Task<Product?> GetProductByNameAsync(string name);
    Task<Product?> GetUserProductByIdAsync(string userId, string productId);

}
