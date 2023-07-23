using ErrorOr;
using MediatR;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.Application.Features.Products.Commands.Edit;

public record EditProductCommand(int ProductId,
                                 string UserId,
                                 bool IsAvailable,
                                 string ManufactureEmail,
                                 string ManufacturePhone,
                                 DateTime ProduceDate,
                                 string Name) : IRequest<ErrorOr<Product>>;
