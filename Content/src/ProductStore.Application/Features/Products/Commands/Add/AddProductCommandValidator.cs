using System.Text.RegularExpressions;
using FluentValidation;

namespace ProductStore.Application.Features.Products.Commands.Add;

public class AddProductCommandValidator :
                 AbstractValidator<AddProductCommand>
{

    public AddProductCommandValidator()
    {

    }
}
