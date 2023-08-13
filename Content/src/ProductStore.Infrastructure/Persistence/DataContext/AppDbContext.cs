using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Infrastructure.Persistence.DataContext;

public class AppDbContext : IdentityUserContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
         base(options)
    {
    }
    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
                    .HasIndex(p => new { p.Id, p.Name })
                    .IsUnique();

        modelBuilder.Entity<Product>()
                    .Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);
        modelBuilder.Entity<Product>()
                    .Property(p => p.Quantity)
                    .IsRequired();



        modelBuilder.Entity<Product>()
                    .Property(p => p.UserId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);



    }
}
