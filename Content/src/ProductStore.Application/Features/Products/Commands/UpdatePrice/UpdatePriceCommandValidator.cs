using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.UpdatePrice;

public class UpdatePriceCommandValidator :
                 AbstractValidator<UpdatePriceCommand>
{

    public UpdatePriceCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                              .WithMessage("something went wrong.. maybe you need to login again");
        RuleFor(x => x.Id).NotEmpty()
                                 .WithMessage("Product Id is required.");

        RuleFor(x => x.price).NotEmpty()
                             .GreaterThanOrEqualTo(0)
                             .WithMessage("Plz enter valid price");

    }
}
