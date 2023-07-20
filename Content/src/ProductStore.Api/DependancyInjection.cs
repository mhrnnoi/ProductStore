namespace ProductStore.Api;

public static class DependancyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
        return services;
    }
}
