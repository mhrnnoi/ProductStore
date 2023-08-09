using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductStore.Api.Filters;

public class AuthFilterAttribute : Attribute, IActionFilter
{
    private readonly ILogger<AuthFilterAttribute> _logger;
    public AuthFilterAttribute(ILogger<AuthFilterAttribute> logger)
    {
        _logger = logger;
    }



    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("this action is related to authentication started");
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("this action is related to authentication finished");

    }

}