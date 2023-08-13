using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{

    public LoginQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
                             .EmailAddress()
                             .WithMessage("plz enter valid email");

        RuleFor(x => x.Password).NotEmpty()
                                .WithMessage("plz enter password");

        //no more validation for password input cuz of security reason
    }
}
