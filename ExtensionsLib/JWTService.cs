using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Services
{
    public class JWTService
    {
        //public string CreateToken(string username)
        //{
        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes("SecretKey123456"));

        //    var creds = new SigningCredentials(
        //        key, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, username)
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: "gatewayapi",
        //        audience: "clientapp",
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler()
        //        .WriteToken(token);
        //}

        private readonly IConfiguration _config;

        // Inject IConfiguration to pull settings safely
        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(string username)
        {
            // 1. Pull the 32+ byte key from appsettings.json or Environment Variables

            var secretKey = _config["Jwt:SecretKey"];
            var issuer = _config["Jwt:Issuer"] ?? "gatewayapi";
            var audience = _config["Jwt:Audience"] ?? "clientapp";
            //var secretKey = "MySuperSecretAndSecureKeyThatIsAtLeast32BytesLong!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // 2. Use UtcNow
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

