using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Api.Filters;
using ProductStore.Application.Features.Authentication.Commands.DeleteAccount;
using ProductStore.Application.Features.Authentication.Commands.Login;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Contracts.Authentication.Requests;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
[AllowAnonymous]
[ServiceFilter(typeof(AuthFilterAttribute))]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;


    public AuthenticationController(IMapper mapper,
                                    ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpPost]
    [ServiceFilter(typeof(RegisterActionFilterAttribute))]

    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteAccountCommand>(request);
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));


    }
}
