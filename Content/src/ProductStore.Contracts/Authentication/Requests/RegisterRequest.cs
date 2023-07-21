namespace ProductStore.Contracts.Authentication.Requests;

public record RegisterRequest(string UserName, string Email, string Password);