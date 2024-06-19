using System.Collections;
using System.Security.Authentication;
using System.Security.Cryptography;
using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace IllegalLibAPI.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthRepository(DataContext context, ILogger logger, TokenGenerator tokenGenerator, UserRepository userRepository)
        {
            _dataContext = context;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<AuthUser> AuthenticateUserAsync(AuthUser authUser, bool hashedPassword = false)
        {
            var existingUser = await _dataContext.AuthUsers
            .FirstOrDefaultAsync(u => u.Username == authUser.Username)
            ?? throw new AuthenticationException("Invalid username or password");

            var passwordIsValid =
            hashedPassword ? existingUser.Password == authUser.Password : BCrypt.Net.BCrypt.Verify(authUser.Password, existingUser.Password);

            if (!passwordIsValid) throw new AuthenticationException("Invalid username or password");

            return existingUser;
        }

        public async Task<AuthUser> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting a user with email {email}");

            var userData = await _dataContext.AuthUsers
            .Where(u => u.Email == email)
            .Include(u => u.User)
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"User with email: {email} does not exist");
            return userData!;
        }

        public async Task RecoverPasswordAsync(string email, string token, string newPassword)
        {
            var user = await GetUserByEmailAsync(email);

            if (user.ResetToken == null)
            {
                throw new InvalidOperationException("User does not have a recovery token.");
            }

            if (user.ResetToken != token)
            {
                throw new InvalidOperationException("Recovery token invalid.");
            }

            user.Password = newPassword;
            await _dataContext.SaveChangesAsync();
        }

        public async Task<string> AssignResetTokenAsync(string email)
        {
            var user = GetUserByEmailAsync(email).Result;
            var token = _tokenGenerator.GenerateResetToken(email);

            if (user.ResetToken != null && user.ResetToken != token)
            {
                throw new InvalidOperationException("Invalid reset token");
            }
            else if (user.ResetToken == token)
            {
                throw new InvalidOperationException("Unexpected error occured: incorrect token");
            }
            await _dataContext.SaveChangesAsync();
            return token;
        }

        public async Task<AuthUser> RegisterUserAsync(RegisterDTO userToRegister)
        {
            var usernameExists = await _dataContext.AuthUsers.AnyAsync(u => u.Username == userToRegister.UserName);
            if (usernameExists) throw new InvalidOperationException($"Username is taken");
            var emailExists = await _dataContext.AuthUsers.AnyAsync(u => u.Email == userToRegister.Email);
            if (emailExists) throw new InvalidOperationException("Account with this email already exists");

            AuthUser authUser = new(userToRegister.UserName, userToRegister.Password, userToRegister.Email);


            try
            {
                await _dataContext.AuthUsers.AddAsync(authUser);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation("User registration successful.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while registering the user.");
                throw;
            }
            await _userRepository.CreateUserAsync(authUser, userToRegister.FirstName, userToRegister.LastName);


            return authUser;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dataContext.AuthUsers.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _dataContext.AuthUsers.AnyAsync(u => u.Email == email);
        }
    }
}