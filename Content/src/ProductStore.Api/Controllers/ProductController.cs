using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Products.Commands.Add;
using ProductStore.Contracts.Products.Requests;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
public class ProductController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediatR;
    public ProductController(IMediator mediatR, IMapper mapper)
    {
        _mediatR = mediatR;
        _mapper = mapper;
    }


    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request)
    {
        var command = _mapper.Map<AddProductCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteProductAsync(int productId)
    // {
    //     var command = new DeleteProductCommand(UserId, productId);
    //     var result = await _mediatR.Send(command);
    //     return result.Match(result => Ok(result),
    //                          errors => Problem(errors));

    // }


}
