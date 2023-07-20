using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Add;

public record AddProductCommand(
    bool IsAvailable,
    string ManufactureEmail,
    string ManufacturePhone,
    DateTime ProduceDate,
    string Name
) : IRequest<ErrorOr<Product>>;
