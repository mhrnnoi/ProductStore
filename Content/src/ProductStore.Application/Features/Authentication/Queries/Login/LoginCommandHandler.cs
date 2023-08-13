using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Domain.Common.Errors;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Domain.Abstractions;

namespace ProductStore.Application.Features.Authentication.Queries.Login;

public class LoginCommandHandler :
                    IRequestHandler<LoginQuery,
                                    ErrorOr<AuthResult>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginCommandHandler(UserManager<IdentityUser> userManager,
                               IMapper mapper,
                               IJwtGenerator jwtGenerator
                               )
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
    }


    public async Task<ErrorOr<AuthResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var managedUser = await _userManager.FindByEmailAsync(request.Email);


        if (managedUser is null)
            return Errors.Auth.InvalidCred;
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser,
                                                             request.Password);
        if (!isPasswordValid)
            return Errors.Auth.InvalidCred;

        var token = _jwtGenerator.GenerateToken(managedUser);

        var authResult = _mapper.Map<AuthResult>(managedUser);
        authResult = authResult with { Token = token };
        return authResult;
    }
}
