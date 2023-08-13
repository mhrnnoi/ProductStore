using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Products.Commands.Add;
using ProductStore.Application.Features.Products.Commands.Delete;
using ProductStore.Application.Features.Products.Commands.Edit;
using ProductStore.Application.Features.Products.Queries.GetAll;
using ProductStore.Application.Features.Products.Queries.GetUserProductsById;
using ProductStore.Contracts.Products.Requests;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
public class ProductController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediatR;
    public ProductController(IMediator mediatR,
                             IMapper mapper)
    {
        _mediatR = mediatR;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync(AddProductRequest request)
    {

        var userId = GetUserId(User.Claims);
        
        var command = _mapper.Map<AddProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductAsync(string productId)
    {

        var userId = GetUserId(User.Claims);
        var command = new DeleteProductCommand(userId, productId);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPut]
    public async Task<IActionResult> EditProductAsync(EditProductRequest request)
    {

        var userId = GetUserId(User.Claims);
        var command = _mapper.Map<EditProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProductsAsync()
    {

        var query = new GetAllProductsQuery();
        var result = await _mediatR.Send(query);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserProductsByIdAsync(string id)
    {
        var query = new GetUserProductsByIdQuery(id);
        var result = await _mediatR.Send(query);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }


}
