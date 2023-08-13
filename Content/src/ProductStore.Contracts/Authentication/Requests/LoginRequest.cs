namespace ProductStore.Contracts.Authentication.Requests;

public record LoginRequest(string Email,
                           string Password);