using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicCQRS.Services.AuthService
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value; // Get the JwtOptions from the appsettings.json based on the JwtOptions class registered in the Program.cs
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
            };  // Create a claim with the username

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)); // Create a symmetric security key using the secret from the JwtOptions
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Create signing credentials using the key and the HMACSHA256 algorithm

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds); // Create a JWT token with the issuer, audience, claims, expiration time, and signing credentials

            return new JwtSecurityTokenHandler().WriteToken(token); // Write the token as a string
        }

        /*
         * This service generates a JWT token using the JwtOptions configured in the appsettings.json file.
         * This is done by injecting the IOptions<JwtOptions> into the TokenService constructor.
         * The GenerateToken method creates a claim with the username, creates a symmetric security key using the secret from the JwtOptions, and creates signing credentials using the key and the HMACSHA256 algorithm.
         * This is a standard way we need to follow to generate a JWT token in ASP.NET Core.
         * Ultimately it is using the injected dependencies to generate the token.
         */
    }
}
