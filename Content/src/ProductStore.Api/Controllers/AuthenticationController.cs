using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Authentication.Commands.Login;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Contracts.Authentication.Requests;

namespace ProductStore.Api.Controllers;

[Route("api/[controller]/[action]")]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediatR;

    public AuthenticationController(IMediator mediatR, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _mediatR = mediatR;
        _mapper = mapper;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }
}