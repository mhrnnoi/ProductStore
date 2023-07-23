using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator :
                 AbstractValidator<DeleteProductCommand>
{

    public DeleteProductCommandValidator()
    {
         RuleFor(x => x.UserId).NotEmpty()
                               .WithMessage("Something went wrong..");
         RuleFor(x => x.ProductId).NotEmpty()
                               .WithMessage("Product Id  is required.");
    }
}
