using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Products.Commands.Add;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
public class ProductController : ApiController
{
    private readonly IMediator _mediatR;
    public ProductController(IMediator mediatR)
    {
        _mediatR = mediatR;
    }


    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request)
    {
        var command = new AddProductCommand();

        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }


}
