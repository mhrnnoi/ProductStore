using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandValidator :
                 AbstractValidator<ResetPasswordCommand>
{

    public ResetPasswordCommandValidator()
    {

    }
}
