using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public class EditProductCommandValidator :
                 AbstractValidator<EditProductCommand>
{

    public EditProductCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                              .WithMessage("something went wrong.. maybe you need to login again");
        RuleFor(x => x.Id).NotEmpty()
                                 .WithMessage("Product Id is required.");

        RuleFor(x => x.quantity).NotEmpty()
                                .GreaterThanOrEqualTo(0)
                                .WithMessage("quantity can't be below zero.");

        RuleFor(x => x.price).NotEmpty()
                             .GreaterThanOrEqualTo(0)
                             .WithMessage("Plz enter valid price");

    }
}
