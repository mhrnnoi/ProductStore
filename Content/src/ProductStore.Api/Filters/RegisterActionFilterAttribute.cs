using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductStore.Api.Filters;

public class RegisterActionFilterAttribute : Attribute, IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}
