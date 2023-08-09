using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ProductStore.Api.Filters;

public class GlobalFilter : IActionFilter
{
    private readonly ILogger<GlobalFilter> _logger;
    public GlobalFilter(ILogger<GlobalFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var endPoint = context.HttpContext.GetEndpoint().DisplayName;
        _logger.LogInformation($"executing an action with this endpoint : {endPoint}");
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var endPoint = context.HttpContext.GetEndpoint().DisplayName;
        _logger.LogInformation($"execution action with this endpoint : {endPoint} finished");
    }
}