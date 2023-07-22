using FluentValidation;

namespace ProductStore.Application.Features.Products.Queries.GetAll;

public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
    }
}
