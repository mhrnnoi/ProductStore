using ErrorOr;
using MediatR;
using ProductStore.Application.Features.Authentication.Common;

namespace ProductStore.Application.Features.Authentication.Queries.Login;

public record LoginQuery(string Email,
                           string Password) : IRequest<ErrorOr<AuthResult>>;
