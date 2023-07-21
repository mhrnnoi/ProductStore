using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public class EditProductCommandValidator :
                 AbstractValidator<EditProductCommand>
{

    public EditProductCommandValidator()
    {

    }
}
