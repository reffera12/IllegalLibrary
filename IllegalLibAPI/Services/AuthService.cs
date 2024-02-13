using System.IdentityModel.Tokens.Jwt;
using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace IllegalLibAPI.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<AuthUser> AuthenticateAsync(AuthUser authUser, bool hashedPassword = false)
        {
            var authuser = await _authRepository.AuthenticateUserAsync(authUser, hashedPassword);

            var key = _configuration.GetSection("JwtSecret").Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("Id", authUser.UserId.ToString()),
                    new Claim("Username", authUser.Username),
                    new Claim(ClaimTypes.Email, authUser.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);
            return authuser;
        }
    }
}