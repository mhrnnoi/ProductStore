using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductStore.Api.Filters;

public class AuthFilterAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        throw new NotImplementedException();
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }

}