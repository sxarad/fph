using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Exercise.Core
{
    public interface ICustomAuthenticationService
    {
        JwtSecurityToken Authenticate(string userName, string password);
    }

    public class CustomAuthenticationService : ICustomAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public CustomAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken Authenticate(string userName, string password)
        {
            // For testing just checking if userName and password values are passed
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("NormalUser", "true"),
                };
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                var secretKeyConfig = _configuration["SecretKey"];
                if (string.IsNullOrWhiteSpace(secretKeyConfig))
                    throw new InvalidOperationException("SecretKey config not defined.");
                var secretKey = Encoding.ASCII.GetBytes(secretKeyConfig);
                var jwt = new JwtSecurityToken(
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiresAt,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature));

                return jwt;
            }

            return new JwtSecurityToken();
        }
    }
}