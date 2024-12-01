using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GastappApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace GastappApi.Utils
{
    public class UserUtils
    {
        private readonly IConfiguration _config;

        public UserUtils(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())

            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("JWT:KEY").Value ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JWT:Key"]!);

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public int GetUserIdFromToken(string token)
        {
            var principals = GetPrincipalFromToken(token);
            if (principals != null)
            {
                if (int.TryParse(principals.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int value))
                {
                    return value;
                }
            }

            return -1;
        }

    }
}