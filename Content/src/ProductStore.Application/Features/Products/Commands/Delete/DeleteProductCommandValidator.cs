using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator :
                 AbstractValidator<DeleteProductCommand>
{

    public DeleteProductCommandValidator()
    {

    }
}
