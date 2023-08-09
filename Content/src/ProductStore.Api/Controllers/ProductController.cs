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
    private readonly ISender _sender;
    public ProductController(IMapper mapper,
                             ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request, CancellationToken cancellationToken)
    {

        var userId = GetUserId(User.Claims);

        var command = _mapper.Map<AddProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductAsync(int productId, CancellationToken cancellationToken)
    {

        var userId = GetUserId(User.Claims);
        var command = new DeleteProductCommand(userId, productId);
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPut]
    public async Task<IActionResult> EditProductAsync([FromBody] EditProductRequest request, CancellationToken cancellationToken)
    {

        var userId = GetUserId(User.Claims);
        var command = _mapper.Map<EditProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProductsAsync(CancellationToken cancellationToken)
    {

        var query = new GetAllProductsQuery();
        var result = await _sender.Send(query, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserProductsByIdAsync(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserProductsByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }


}
