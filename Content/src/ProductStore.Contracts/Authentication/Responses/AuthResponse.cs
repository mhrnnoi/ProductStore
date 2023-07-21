namespace ProductStore.Contracts.Authentication.Responses;

public record AuthResponse(string Email, string UserName, string Token);