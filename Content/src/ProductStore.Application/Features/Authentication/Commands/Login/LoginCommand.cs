using ErrorOr;
using MediatR;
using ProductStore.Application.Features.Authentication.Common;

namespace ProductStore.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email,string Password) : IRequest<ErrorOr<AuthResult>>;
