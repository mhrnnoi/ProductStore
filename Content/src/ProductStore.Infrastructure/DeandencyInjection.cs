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
using Serilog.Events;

namespace ProductStore.Infrastructure;

public static class depandencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                        ConfigurationManager configurationManager,
                                                        ILoggingBuilder loggingBuilder)
    {
        AddLogger(configurationManager, loggingBuilder);

        JwtSettings jwtSettings = AddJwtSettings(services, configurationManager);

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
            Options.UseSqlServer(GetDefaultConnection(configurationManager));
        });

        services.AddMapping();



        return services;
    }

    private static JwtSettings AddJwtSettings(IServiceCollection services, ConfigurationManager configurationManager)
    {
        var jwtSettings = new JwtSettings();
        configurationManager.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        return jwtSettings;
    }
    private static string GetDefaultConnection(ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString(ConnectionStringSettings.DefaultConnection);
        return connectionString;
    }

    private static void AddLogger(ConfigurationManager configurationManager, ILoggingBuilder loggingBuilder)
    {
        var logger = new LoggerConfiguration().ReadFrom.Configuration(configurationManager)
                                                      .Enrich.FromLogContext()
                                                      .MinimumLevel.Information()
                                                      .CreateLogger();
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(logger);
        
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
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
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

