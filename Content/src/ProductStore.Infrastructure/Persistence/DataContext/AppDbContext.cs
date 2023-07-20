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
            options.UseSqlServer("Server=localhost;Initial Catalog=mehran; User Id=SA; Password=Mehrancsharp6690;Encrypt=false");
        }

    }
}
