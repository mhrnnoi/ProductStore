using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{

    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
                             .EmailAddress()
                             .WithMessage("plz enter valid email");

        RuleFor(x => x.Password).NotEmpty()
                                .WithMessage("plz enter password");

        //no more validation for password input cuz of security reason
    }
}
