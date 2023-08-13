using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public record EditProductCommand(string Id,
                                 string UserId,
                                 int quantity,
                                 decimal price) : IRequest<ErrorOr<Product>>;
