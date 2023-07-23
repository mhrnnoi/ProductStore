using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler :
                    IRequestHandler<LoginCommand,
                                    ErrorOr<AuthResult>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginCommandHandler(IUnitOfWork unitOfWork,
                                    UserManager<IdentityUser> userManager,
                                    IMapper mapper,
                                    IJwtGenerator jwtGenerator)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
    }


    public async Task<ErrorOr<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var managedUser = await _userManager.FindByEmailAsync(request.Email);

        if (managedUser is null)
            return Error.Failure("Bad Credential");

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser,
                                                                    request.Password);
        if (!isPasswordValid)
            return Error.Failure("Bad Credential");

        var token = _jwtGenerator.GenerateToken(managedUser);
        await _unitOfWork.SaveChangesAsync();
        var authResult = _mapper.Map<AuthResult>(managedUser);
        authResult = authResult with { Token = token };
        return authResult;
    }
}
