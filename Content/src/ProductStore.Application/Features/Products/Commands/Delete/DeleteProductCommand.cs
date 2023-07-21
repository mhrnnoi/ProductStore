using ErrorOr;
using MediatR;

namespace ProductStore.Application.Features.Products.Commands.Delete;

public record DeleteProductCommand(
    int UserId,
    int ProductId
) : IRequest<ErrorOr<bool>>;
