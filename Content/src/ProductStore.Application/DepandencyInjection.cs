using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductStore.Application.Behaviours;
using FluentValidation;

namespace ProductStore.Application;

public static class DepandencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly
                                    (Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>),
                                 typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
