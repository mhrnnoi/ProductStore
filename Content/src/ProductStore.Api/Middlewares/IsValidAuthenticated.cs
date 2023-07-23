namespace ProductStore.Api.Middlewares;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using ProductStore.Application.Interfaces.Persistence;

public class IsValidAuthenticated : IMiddleware
{
    private readonly ICacheService _cacheService;

    public IsValidAuthenticated(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var token = await context.GetTokenAsync("Bearer", "access_token");
            var isBlacklist = _cacheService.IsInBlacklist(token);
            if (isBlacklist)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await next(context);
    }
}