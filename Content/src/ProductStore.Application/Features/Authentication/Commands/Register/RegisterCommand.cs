using ErrorOr;
using MediatR;
using ProductStore.Application.Features.Authentication.Common;

namespace ProductStore.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string Email, string UserName, string Password
) : IRequest<ErrorOr<AuthResult>>;
