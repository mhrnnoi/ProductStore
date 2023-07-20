using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Infrastructure.Persistence.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
         base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
}
