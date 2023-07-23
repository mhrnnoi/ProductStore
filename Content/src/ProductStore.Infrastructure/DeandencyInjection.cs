using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Infrastructure.Mapping;
using ProductStore.Infrastructure.Persistence;
using ProductStore.Infrastructure.Persistence.DataContext;
using ProductStore.Infrastructure.Services;
using Serilog;

namespace ProductStore.Infrastructure;

public static class depandencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                        ConfigurationManager configurationManager)
    {
        var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configurationManager)
                    .Enrich.FromLogContext()
                    .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders()
                          .AddSerilog(logger);
        });


        var jwtSettings = new JwtSettings();
        configurationManager.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IDateTimeProvider, DatetimeProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddAuthentication(AuthScheme())
                                  .AddJwtBearer(JwtBearerOptions(jwtSettings));



        services.AddDbContext<AppDbContext>(Options =>
        {
            Options.UseSqlServer("Server=sqlserver;Database=ProductStore;User Id=SA;Password=Passwordcomplex6690;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        });

        services.AddMapping();



        return services;
    }

    private static Action<AuthenticationOptions> AuthScheme()
    {
        return option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        };
    }

    private static Action<JwtBearerOptions> JwtBearerOptions(JwtSettings jwtSettings)
    {
        return options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        };
    }

}

