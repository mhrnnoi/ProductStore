using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Infrastructure.Persistence;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure;

public static class depandencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        services.AddDbContext<AppDbContext>(Options =>
        {
            Options.UseSqlServer("");
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}

