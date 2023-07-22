using FluentValidation;

namespace ProductStore.Application.Features.Products.Queries.GetUserProductsById;

public class GetUserProductsByIdQueryValidator : AbstractValidator<GetUserProductsByIdQuery>
{
    public GetUserProductsByIdQueryValidator()
    {
    }
}
