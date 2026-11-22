using Ecommerce.Data;
using Ecommerce.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;   
using Ecommerce.Dtos;    

namespace Ecommerce.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user)
        {
            // 1. Read JWT configuration

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing.");

            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is missing.");

            var expiryMinutes = _configuration.GetValue<int?>("Jwt:ExpirationMinutes")
                ?? throw new InvalidOperationException("JWT ExpirationMinutes is missing.");

            // 2. Validate configuration

            if (expiryMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "JWT expiration must be greater than zero.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT key must contain at least 32 bytes for HS256.");
            }

            // 3. Create security key

            var securityKey = new SymmetricSecurityKey(keyBytes);

            // 4. Create signing credentials

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            // 5. Create claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // 6. Create JWT

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // 7. Convert JWT into string

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}