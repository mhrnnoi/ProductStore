using ErrorOr;
using MediatR;
using ProductStore.Application.Features.Authentication.Common;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public record DeleteAccountCommand(string Token,
                                   string Email,
                                   string Password) : IRequest<ErrorOr<bool>>;
