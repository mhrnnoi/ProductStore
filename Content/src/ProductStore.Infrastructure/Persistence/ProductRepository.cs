using Microsoft.EntityFrameworkCore;
using ProductStore.Application.Interfaces.Persistence;
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

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> IsEmailAndDateUniqueAsync(string email, DateTime date)
    {
        var isNotUnique = await _context.AnyAsync(
                x => x.ManufactureEmail == email &&
                        x.ProduceDate == date);
        if (isNotUnique)
        {
            return false;
        }
        return true;
    }

    public void Remove(Product entity)
    {
        _context.Remove(entity);
    }

    public void Update(Product entity)
    {
        _context.Update(entity);

    }



    public Task<List<Product>> UserProductsAsync(int userId)
    {
        throw new NotImplementedException();
    }


    public Task<Product?> UserProductsByIdAsync(int userId, int productId)
    {
        throw new NotImplementedException();
    }
}