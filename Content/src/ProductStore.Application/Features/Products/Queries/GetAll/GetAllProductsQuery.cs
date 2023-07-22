using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Queries.GetAll;

public record GetAllProductsQuery() : IRequest<ErrorOr<List<Product>>>;
