using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public class DeleteAccountCommandHandler :
                    IRequestHandler<DeleteAccountCommand,
                                    ErrorOr<bool>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICacheService _cacheService;

    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork,
                                    UserManager<IdentityUser> userManager,
                                    IMapper mapper,
                                    IJwtGenerator jwtGenerator,
                                    IDateTimeProvider dateTimeProvider,
                                    ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenerator = jwtGenerator;
        _dateTimeProvider = dateTimeProvider;
        _cacheService = cacheService;
    }


    public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.Failure();

        }
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
        {
            return Error.Failure();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {

            var firstError = result.Errors.First();
            return Error.Failure(description: firstError.Description,
                                     code: firstError.Code);
        }
        _cacheService.AddBlacklist(request.Token, _dateTimeProvider.UtcNow.AddMinutes(5));

        await _unitOfWork.SaveChangesAsync();
        return true;


    }
}
