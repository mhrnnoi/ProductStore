using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator :
                 AbstractValidator<RegisterCommand>
{
    //asp.net identity check validation too like existing email or user name, password validation too
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty()
                                .WithMessage("plz enter username");
        RuleFor(x => x.Email).NotEmpty()
                             .EmailAddress()
                             .WithMessage("plz enter valid email");
        RuleFor(x => x.Password).NotEmpty()
                                .MinimumLength(8)
                                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")
        .WithMessage("password must have Minimum eight characters, at least one upper case English letter, one lower case English letter, one number and one special character");
    }
}
