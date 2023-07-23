namespace ProductStore.Contracts.Authentication.Requests;

public record DeleteAccountRequest(string Email, string Password);