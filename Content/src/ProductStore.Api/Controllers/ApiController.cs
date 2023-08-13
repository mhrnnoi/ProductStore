using System.Security.Claims;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Api.Common.Http;

namespace ProductStore.Api.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    protected string GetUserId(IEnumerable<Claim> claims) => 
            claims.First(a => a.Type == ClaimTypes.NameIdentifier).Value;



    protected IActionResult Problem(List<Error> errors)
    {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        var firstError = errors.First();

        var myStatusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: myStatusCode,
                         title: firstError.Description);
    }
}
