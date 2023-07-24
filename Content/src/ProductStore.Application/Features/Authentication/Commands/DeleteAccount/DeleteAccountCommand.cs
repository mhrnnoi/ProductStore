using ErrorOr;
using MediatR;

namespace ProductStore.Application.Features.Authentication.Commands.DeleteAccount;

public record DeleteAccountCommand(string Email,
                                   string Password) : IRequest<ErrorOr<bool>>;
