using Microsoft.AspNetCore.Identity;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;
using ProductStore.Infrastructure.Persistence.DataContext;

namespace ProductStore.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<IdentityUser?> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser is null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser,
                                                                    password);
        if (!isPasswordValid)
            return null;

        return managedUser;
    }

    public Task<IdentityUser?> Register(string Email, string UserName, string Password)
    {
        throw new NotImplementedException();
    }
}