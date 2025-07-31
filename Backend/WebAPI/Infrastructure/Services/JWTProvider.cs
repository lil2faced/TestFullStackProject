using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Interfaces;

namespace WebAPI.Infrastructure.Services
{
    public class JWTProvider : IJWTProvider
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _tokenHours;

        public JWTProvider(IConfiguration configuration)
        {
            _secretKey = configuration["JWT:KEY"]
                ?? throw new InvalidOperationException("JWT Key not configured");
            _issuer = configuration["JWT:ISSUER"]
                ?? throw new InvalidOperationException("JWT Issuer not configured");
            _audience = configuration["JWT:AUDIENCE"]
                ?? throw new InvalidOperationException("JWT Audience not configured");
            _tokenHours = double.Parse(configuration["JWT:HOURS"]
                ?? throw new InvalidOperationException("JWT Hours not configured"));
        }
        public string GenerateToken(DTOUserAPIJwt user)
        {
            var claims = new List<Claim>
            {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.Role.Name) // Используем Name вместо Role
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_tokenHours),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
