using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Infrastructure.Mapping;
using ProductStore.Infrastructure.Persistence;
using ProductStore.Infrastructure.Persistence.DataContext;
using ProductStore.Infrastructure.Services;

namespace ProductStore.Infrastructure;

public static class depandencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                        ConfigurationManager configurationManager)
    {
        var jwtSettings = new JwtSettings();
        configurationManager.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IDateTimeProvider, DatetimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtOptions(jwtSettings));

        services.AddDbContext<AppDbContext>(Options =>
        {
            Options.UseSqlServer(ConnectionString());
        });
        services.AddMapping();



        return services;
    }

    private static string ConnectionString()
    {
        return "Server=sqlserver;Database=ProductStore;User Id=SA;Password=Passwordcomplex6690;MultipleActiveResultSets=true;TrustServerCertificate=True;";
    }

    private static Action<JwtBearerOptions> JwtOptions(JwtSettings jwtSettings)
    {
        return options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        };
    }
}

