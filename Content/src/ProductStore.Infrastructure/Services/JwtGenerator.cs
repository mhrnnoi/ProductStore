using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Infrastructure.Services;

    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSettings _jwtOptions;
        private IDateTimeProvider _dateTimeProvider;

        public JwtGenerator(IOptions<JwtSettings> jwtOptions,
                                IDateTimeProvider dateTimeProvider)
        {
            _jwtOptions = jwtOptions.Value;
            _dateTimeProvider = dateTimeProvider;
        }
        public string GenerateToken(IdentityUser user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
            var mySigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha512);


            var myCliaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"Token"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, _dateTimeProvider.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            
            var securityToken = new JwtSecurityToken(claims: myCliaims,
                                                     audience: _jwtOptions.Audience,
                                                     signingCredentials: mySigningCredentials,
                                                     expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes));


            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(securityToken);
        }
    }
