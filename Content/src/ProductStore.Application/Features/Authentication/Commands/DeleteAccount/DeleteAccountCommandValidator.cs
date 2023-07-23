using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public class DeleteAccountCommandValidator :
                 AbstractValidator<DeleteAccountCommand>
{

    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
                            .EmailAddress()
                            .WithMessage("plz enter valid email");
        RuleFor(x => x.Password).NotEmpty()
                                .WithMessage("plz enter password");
       
        RuleFor(x => x.Token).NotEmpty()
                             .WithMessage("failed to delete account");
    }
}
