using System.IdentityModel.Tokens.Jwt;
using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using IllegalLibAPI.Data;

namespace IllegalLibAPI.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly DataContext _dataContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IUserRepository userRepository, DataContext dataContext, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _dataContext = dataContext;
            _logger = logger;
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
        public async Task<AuthUser> RegisterUserAsync(RegisterDTO userToRegister)
        {
            var usernameExists = await _authRepository.IsUsernameTakenAsync(userToRegister.UserName);
            if (usernameExists)
            {
                throw new InvalidOperationException($"Username '{userToRegister.UserName}' is already taken.");
            }

            // Check if email is taken
            var emailExists = await _authRepository.IsEmailTakenAsync(userToRegister.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email '{userToRegister.Email}' is already registered.");
            }

            // Create AuthUser entity
            var authUser = new AuthUser(userToRegister.UserName, userToRegister.Password, userToRegister.Email);

            // Perform both operations in a transaction
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    await _authRepository.RegisterUserAsync(userToRegister);
                    await _userRepository.CreateUserAsync(authUser, userToRegister.FirstName, userToRegister.LastName);

                    await _dataContext.SaveChangesAsync();

                    transaction.Commit();
                    _logger.LogInformation("User registration successful.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Error occurred while registering the user.");
                    throw;
                }
            }

            return authUser;
        }
    }
}