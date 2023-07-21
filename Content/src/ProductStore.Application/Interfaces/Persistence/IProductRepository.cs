using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Interfaces.Persistence;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<bool> IsEmailAndDateUniqueAsync(string email, DateTime date);
    Task<List<Product>> GetUserProductsAsync(int userId);
    Task<Product?> GetUserProductByIdAsync(int userId, int productId);

}
