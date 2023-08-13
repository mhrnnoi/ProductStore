using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Add;

public class AddProductCommandValidator :
                 AbstractValidator<AddProductCommand>
{

    public AddProductCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                              .WithMessage("something went wrong.. maybe you need to login again");


        RuleFor(x => x.quantity).NotEmpty()
                                .GreaterThanOrEqualTo(0)
                                .WithMessage("quantity can't be below zero.");

        RuleFor(x => x.price).NotEmpty()
                             .GreaterThanOrEqualTo(0)
                             .WithMessage("Plz enter valid price");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
