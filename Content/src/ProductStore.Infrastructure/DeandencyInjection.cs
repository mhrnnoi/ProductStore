using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineVeterinary.Infrastructure.Mapping;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Infrastructure.Persistence;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure;

public static class depandencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMapping();
        services.AddDbContext<AppDbContext>(Options =>
        {
            Options.UseSqlServer("Server=localhost;Port=1433;Initial Catalog=mehran; User Id=sa; Password=Mehrancsharp6690;Encrypt=false");
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}

