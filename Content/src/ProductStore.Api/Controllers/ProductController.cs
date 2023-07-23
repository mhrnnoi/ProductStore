using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Products.Commands.Add;
using ProductStore.Application.Features.Products.Commands.Delete;
using ProductStore.Application.Features.Products.Commands.Edit;
using ProductStore.Application.Features.Products.Queries.GetAll;
using ProductStore.Application.Features.Products.Queries.GetUserProductsById;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Contracts.Products.Requests;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
public class ProductController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediatR;
    private readonly ICacheService _cacheService;
    public ProductController(IMediator mediatR, IMapper mapper, ICacheService cacheService)
    {
        _mediatR = mediatR;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request)
    {
        //  var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        // var isBlacklist =  _cacheService.IsBlacklist(token);
        // if (isBlacklist)
        //     return Unauthorized();
        var userId = GetUserId(User.Claims);
        var command = _mapper.Map<AddProductCommand>(request);
        command = command with { UserId = userId };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpDelete("{productId}")]
    [Authorize]
    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
        // var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        // var isBlacklist =  _cacheService.IsBlacklist(token);
        var userId = GetUserId(User.Claims);
        var command = new DeleteProductCommand(userId, productId);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditProductAsync([FromBody] EditProductRequest request)
    {
        // var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        // var isBlacklist =  _cacheService.IsBlacklist(token);
        // if (isBlacklist)
        //     return Unauthorized();
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
