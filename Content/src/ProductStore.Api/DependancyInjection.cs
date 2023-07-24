using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using ProductStore.Infrastructure.Persistence.DataContext;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductStore.Api;


public static class DependancyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        //add token validation middle ware

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(AddAuthorizeToSwagger());
        services.AddAuthorization();


        services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<AppDbContext>();

        services.Configure(IdenityOptions());

        return services;
    }

    private static Action<SwaggerGenOptions> AddAuthorizeToSwagger()
    {
        return option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        };

    }

    private static Action<IdentityOptions> IdenityOptions()
    {
        return options =>
        {

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;


            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        };
    }
}
