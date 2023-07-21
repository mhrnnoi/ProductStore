using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Products.Commands.Add;
using ProductStore.Application.Features.Products.Commands.Delete;
using ProductStore.Application.Features.Products.Commands.Edit;
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
        var userId = GetUserId(User.Claims);
        var command = _mapper.Map<AddProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
        var userId = GetUserId(User.Claims);
        var command = new DeleteProductCommand(userId, productId);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }
    [HttpPut()]
    public async Task<IActionResult> DeleteProductAsync([FromBody] EditProductRequest request)
    {
        var userId = GetUserId(User.Claims);
        var command = _mapper.Map<EditProductCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }


}
