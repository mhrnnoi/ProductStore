using ProductStore.Domain.Abstractions;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }


    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();

    }
}
