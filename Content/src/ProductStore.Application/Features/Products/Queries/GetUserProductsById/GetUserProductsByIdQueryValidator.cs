using FluentValidation;

namespace ProductStore.Application.Features.Products.Queries.GetUserProductsById;

public class GetUserProductsByIdQueryValidator : AbstractValidator<GetUserProductsByIdQuery>
{
    public GetUserProductsByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
                               .WithMessage("plz enter valid user id");
    }
}
