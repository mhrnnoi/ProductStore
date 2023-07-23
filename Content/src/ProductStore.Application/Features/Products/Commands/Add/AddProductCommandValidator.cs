using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Add;

public class AddProductCommandValidator :
                 AbstractValidator<AddProductCommand>
{

    public AddProductCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                              .WithMessage("Something went wrong..");

        RuleFor(x => x.IsAvailable).NotNull()
                                   .WithMessage("Availability must be specified.");

        RuleFor(x => x.ManufactureEmail).NotEmpty()
                                        .EmailAddress()
                                        .WithMessage("Plz enter valid email.");

        RuleFor(x => x.ManufacturePhone).NotEmpty()
                                        .MinimumLength(6)
                                        .WithMessage("Plz enter valid Phone number.");

        RuleFor(x => x.ProduceDate).NotEmpty()
                                   .WithMessage("ProduceDate is required.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
