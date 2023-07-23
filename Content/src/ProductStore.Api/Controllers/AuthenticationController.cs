using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Application.Features.Authentication.Commands.DeleteAccount;
using ProductStore.Application.Features.Authentication.Commands.Login;
using ProductStore.Application.Features.Authentication.Commands.Logout;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Application.Features.Authentication.Commands.ResetPassword;
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
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));

    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        var command = new LogoutCommand(token);
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountRequest request)
    {
        var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        var command = _mapper.Map<DeleteAccountCommand>(request);
        command = command with { Token = token! };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        var command = _mapper.Map<ResetPasswordCommand>(request);
        command = command with { Token = token! };
        var result = await _mediatR.Send(command);
        return result.Match(result => Ok(result),
                             errors => Problem(errors));
    }
}
