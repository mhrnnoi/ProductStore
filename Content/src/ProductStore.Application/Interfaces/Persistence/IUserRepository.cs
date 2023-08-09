using Microsoft.AspNetCore.Identity;

namespace ProductStore.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<IdentityUser?> LoginAsync(string email, string password);
}