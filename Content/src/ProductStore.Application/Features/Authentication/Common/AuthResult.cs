namespace ProductStore.Application.Features.Authentication.Common;

public record AuthResult(string Email, string UserName, string Token);