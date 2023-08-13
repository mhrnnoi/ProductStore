using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Add;

public sealed record AddProductCommand(string UserId,
                                int quantity,
                                decimal price,
                                string Name) : IRequest<ErrorOr<Product>>;
