using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler :
                    IRequestHandler<ResetPasswordCommand,
                                    ErrorOr<AuthResult>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly ICacheService _cacheService;

    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork,
                                    UserManager<IdentityUser> userManager,
                                    IMapper mapper,
                                    IJwtGenerator jwtGenerator,
                                    ICacheService cacheService,
                                    IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrorOr<AuthResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Error.Failure("Bad Credential");

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!isPasswordCorrect)
            return Error.Failure("Bad Credential");

        var result = await _userManager.ChangePasswordAsync(user,
                                                            request.OldPassword,
                                                            request.NewPassword);
        if (!result.Succeeded)
            return PasswordChangeFailure(result);

        var blacklistResult = _cacheService.AddToBlacklist(request.Token);
        if (blacklistResult is false)
            return Error.Failure("Something Went Wrong..");

        var newToken = _jwtGenerator.GenerateToken(user);
        var authResult = _mapper.Map<AuthResult>(user);
        authResult = authResult with { Token = newToken };
        await _unitOfWork.SaveChangesAsync();
        return authResult;


    }

    private static ErrorOr<AuthResult> PasswordChangeFailure(IdentityResult result)
    {
        var firstError = result.Errors.First();
        return Error.Failure(description: firstError.Description,
                                 code: firstError.Code);
    }
}
