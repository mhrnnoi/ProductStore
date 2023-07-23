using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandValidator :
                 AbstractValidator<ResetPasswordCommand>
{

    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
                             .EmailAddress()
                             .WithMessage("plz enter valid email");

        RuleFor(x => x.OldPassword).NotEmpty()
                                   .WithMessage("plz enter your current password");;

        RuleFor(x => x.NewPassword).NotEmpty()
                                   .MinimumLength(8)
                                   .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")
        .WithMessage(@"password must have Minimum eight characters, at least one upper case English letter,
                           one lower case English letter, one number and one special character");

        RuleFor(x => x.Token).NotEmpty()
                             .WithMessage("Failed Reset Password");
    }
}
