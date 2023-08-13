using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.UpdatePrice;

public record UpdatePriceCommand(string Id,
                                 string UserId,
                                 decimal price) : IRequest<ErrorOr<Product>>;
