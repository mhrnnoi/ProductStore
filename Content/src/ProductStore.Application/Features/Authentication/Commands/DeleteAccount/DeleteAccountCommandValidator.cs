using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public class DeleteAccountCommandValidator :
                 AbstractValidator<DeleteAccountCommand>
{

    public DeleteAccountCommandValidator()
    {

    }
}
