using ErrorOr;
using MediatR;
using ProductStore.Application.Features.Authentication.Common;

namespace ProductStore.Application.Features.Authentication.Commands.ResetPassword;

public record ResetPasswordCommand(string Token,
                                   string Email,
                                   string OldPassword,
                                   string NewPassword) : IRequest<ErrorOr<AuthResult>>;
