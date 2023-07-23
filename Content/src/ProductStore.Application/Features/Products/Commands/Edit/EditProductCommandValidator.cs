using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public class EditProductCommandValidator :
                 AbstractValidator<EditProductCommand>
{

    public EditProductCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                              .WithMessage("Something went wrong..");
        RuleFor(x => x.ProductId).NotEmpty()
                                 .WithMessage("Product Id is required.");

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
