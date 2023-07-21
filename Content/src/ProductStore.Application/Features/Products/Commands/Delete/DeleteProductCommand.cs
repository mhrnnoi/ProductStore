using ErrorOr;
using MediatR;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public record DeleteProductCommand(
    string UserId,
    int ProductId
) : IRequest<ErrorOr<bool>>;
