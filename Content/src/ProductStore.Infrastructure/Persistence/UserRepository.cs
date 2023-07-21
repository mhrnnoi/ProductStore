using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly DbSet<IdentityUser> _context;
    public UserRepository(AppDbContext context)
    {
        _context = context.Set<IdentityUser>();
    }

    public async Task<IdentityUser?> FindByEmailAsync(string email)
    {
        return await _context.FirstOrDefaultAsync(x => x.Email == email); 
    }
}