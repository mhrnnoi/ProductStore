using System.Security.Claims;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ProductStore.Api.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    protected string GetUserId(IEnumerable<Claim> claims)
    {

        return claims.First(a => a.Type == "id").Value;
                                                
    }

    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors.First();

        var myStatusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status422UnprocessableEntity,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unexpected => StatusCodes.Status405MethodNotAllowed,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: myStatusCode,
                         title: firstError.Description);
    }
}
