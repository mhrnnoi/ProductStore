using ErrorOr;
using MediatR;

namespace ProductStore.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string Token) : IRequest<ErrorOr<bool>>;
