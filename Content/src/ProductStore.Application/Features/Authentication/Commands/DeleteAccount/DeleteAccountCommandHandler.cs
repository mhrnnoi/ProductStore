using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public class DeleteAccountCommandHandler :
                    IRequestHandler<DeleteAccountCommand,
                                    ErrorOr<bool>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _chacheService;

    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork,
                                       UserManager<IdentityUser> userManager,
                                       ICacheService chacheService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _chacheService = chacheService;
    }


    public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Error.Failure(description: "Bad Credential");

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
            return Error.Failure(description: "Bad Credential");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return FailToDelete(result);

        _chacheService.RemoveData("products");
        await _unitOfWork.SaveChangesAsync();
        return true;


    }

    private static ErrorOr<bool> FailToDelete(IdentityResult result)
    {
        var firstError = result.Errors.First();
        return Error.Failure(description: firstError.Description,
                                 code: firstError.Code);
    }
}
