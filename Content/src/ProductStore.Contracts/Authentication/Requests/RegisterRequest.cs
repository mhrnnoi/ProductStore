using System.ComponentModel.DataAnnotations;

namespace ProductStore.Contracts.Authentication.Requests;

public record RegisterRequest(string UserName, string Email, string Password)
{
    [Required]
    [Compare(nameof(Password))]
    public string RepeatPassword { get; set; }
}