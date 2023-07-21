using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Infrastructure.Persistence.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }
    public AppDbContext(DbContextOptions<AppDbContext> options) :
         base(options)
    {
    }
    public DbSet<Product> Products { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        if (options.IsConfigured == false)
        {
            options.UseSqlServer("Server=sqlserver;Database=ProductStore;User Id=SA;Password=Passwordcomplex6690;MultipleActiveResultSets=true;TrustServerCertificate=True;Integrated Security=true;");
        }

    }
}
