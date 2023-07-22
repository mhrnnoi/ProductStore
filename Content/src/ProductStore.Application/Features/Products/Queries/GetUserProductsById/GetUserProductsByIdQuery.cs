using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Queries.GetUserProductsById;

public record GetUserProductsByIdQuery(string UserId) : IRequest<ErrorOr<List<Product>>>;
