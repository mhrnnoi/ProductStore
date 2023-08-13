using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.UpdateQuantity;

public record UpdateQuantityCommand(string Id,
                                    string UserId,
                                    int quantity) : IRequest<ErrorOr<Product>>;
