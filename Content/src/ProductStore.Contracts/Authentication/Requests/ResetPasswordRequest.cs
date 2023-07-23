namespace ProductStore.Contracts.Authentication.Requests;

public record ResetPasswordRequest(string Email,
                                   string OldPassword,
                                   string NewPassword);