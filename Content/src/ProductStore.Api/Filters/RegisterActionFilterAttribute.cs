using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductStore.Api.Filters;

public class RegisterActionFilterAttribute : Attribute, IAsyncActionFilter
{
    private readonly ILogger<AuthFilterAttribute> _logger;
    public RegisterActionFilterAttribute(ILogger<AuthFilterAttribute> logger)
    {
        _logger = logger;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("an attempt was made to register a user");
        await next();
        _logger.LogInformation("finished registration process");



    }
}
