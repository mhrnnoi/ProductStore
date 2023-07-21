using FluentValidation;

namespace ProductStore.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator :
                 AbstractValidator<RegisterCommand>
{

    public RegisterCommandValidator()
    {

    }
}
