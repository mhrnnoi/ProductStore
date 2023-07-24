using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator :
                 AbstractValidator<DeleteProductCommand>
{

    public DeleteProductCommandValidator()
    {
         RuleFor(x => x.UserId).NotEmpty()
                               .WithMessage("something went wrong.. maybe you need to login again");
         RuleFor(x => x.ProductId).NotEmpty()
                               .WithMessage("Product Id  is required.");
    }
}
