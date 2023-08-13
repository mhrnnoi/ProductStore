using Microsoft.EntityFrameworkCore;
using ProductStore.Domain.Abstractions;
using ProductStore.Domain.Products.Entities;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly DbSet<Product> _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context.Set<Product>();
    }
    public void Add(Product entity)
    {
        _context.Add(entity);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.ToListAsync();

    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _context.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(Product entity)
    {
        _context.Remove(entity);
    }

    public void Update(Product entity)
    {
        _context.Update(entity);

    }



    public async Task<List<Product>> GetUserProductsAsync(string userId)
    {
        return await _context.Where(x => x.UserId == userId)
                              .ToListAsync();
    }


    public async Task<Product?> GetUserProductByIdAsync(string userId, string productId)
    {
        return await _context.FirstOrDefaultAsync(x => x.UserId == userId &&
                                                    x.Id == productId);
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _context.FirstOrDefaultAsync(x => x.Name == name);
    }
}