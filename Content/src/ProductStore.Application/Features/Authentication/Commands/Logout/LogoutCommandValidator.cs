using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandValidator :
                 AbstractValidator<LogoutCommand>
{

    public LogoutCommandValidator()
    {

    }
}