using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler :
                    IRequestHandler<RegisterCommand,
                                    ErrorOr<AuthResult>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;

    public RegisterCommandHandler(IUnitOfWork unitOfWork,
                                  UserManager<IdentityUser> userManager,
                                  IMapper mapper,
                                  IJwtGenerator jwtGenerator)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
    }


    public async Task<ErrorOr<AuthResult>> Handle(RegisterCommand request,
                                                  CancellationToken cancellationToken)
    {

        var user = _mapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return ReturnRegisterationFailure(result);

        var token = _jwtGenerator.GenerateToken(user);
        var authResult = _mapper.Map<AuthResult>(request);
        authResult = authResult with { Token = token };
        await _unitOfWork.SaveChangesAsync();
        return authResult;


    }

    private static Error ReturnRegisterationFailure(IdentityResult result)
    {
        var firstError = result.Errors.First();
        return Error.Failure(description: firstError.Description,
                             code: firstError.Code);
    }
}
