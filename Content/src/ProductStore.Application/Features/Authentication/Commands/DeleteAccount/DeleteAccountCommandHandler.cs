using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductStore.Domain.Common.Errors;
using ProductStore.Domain.Abstractions;

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
            return Errors.Auth.InvalidCred;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user,
                                                             request.Password);
        if (!isPasswordValid)
            return Errors.Auth.InvalidCred;

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
        return Error.Validation(description: firstError.Description,
                                 code: firstError.Code);
    }
}
