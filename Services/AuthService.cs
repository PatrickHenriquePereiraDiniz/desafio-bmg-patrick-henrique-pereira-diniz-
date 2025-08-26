using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DesafioBMG.Models;
using DesafioBMG.Repositories;

namespace DesafioBMG.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public string? Login(string email, string password)
        {
            var user = _userRepo.GetByEmailAndPassword(email, password);
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return "Bearer " + tokenHandler.WriteToken(token);
        }
    }
}
