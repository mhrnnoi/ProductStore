using Microsoft.AspNetCore.Identity;

namespace ProductStore.Application.Interfaces.Services;

public interface IJwtGenerator
{
    string GenerateToken(IdentityUser user);
}
