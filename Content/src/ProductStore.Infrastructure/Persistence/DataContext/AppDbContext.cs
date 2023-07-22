using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Infrastructure.Persistence.DataContext;

public class AppDbContext : IdentityUserContext<IdentityUser>
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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Id, p.ManufactureEmail, p.ProduceDate })
            .IsUnique();

        modelBuilder.Entity<Product>()
          .Property(p => p.ManufactureEmail)
          .IsRequired()
          .HasMaxLength(255);

        modelBuilder.Entity<Product>()
          .Property(p => p.ManufacturePhone)
          .IsRequired()
          .HasMaxLength(255);

        modelBuilder.Entity<Product>()
            .Property(p => p.ProduceDate)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.UserId)
            .IsRequired();
            
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Product>()
        .HasOne<IdentityUser>()
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);



    }
}
