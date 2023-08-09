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
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IUnitOfWork unitOfWork,
                               UserManager<IdentityUser> userManager,
                               IMapper mapper,
                               IJwtGenerator jwtGenerator,
                               IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
        _userRepository = userRepository;
    }


    public async Task<ErrorOr<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var managedUser = await _userRepository.LoginAsync(request.Email, request.Password);

        if (managedUser is null)
            return Error.Failure(description: "Bad Credential");

        var token = _jwtGenerator.GenerateToken(managedUser);

        await _unitOfWork.SaveChangesAsync();
        var authResult = _mapper.Map<AuthResult>(managedUser);
        authResult = authResult with { Token = token };
        return authResult;
    }
}
