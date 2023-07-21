using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator :
                 AbstractValidator<LoginCommand>
{

    public LoginCommandValidator()
    {

    }
}
