using System.Security.Authentication;
using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IllegalLibAPI.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        public AuthRepository(DataContext context, ILogger logger)
        {
            _dataContext = context;
            _logger = logger;
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

        public Task RecoverPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthUser> RegisterUserAsync(AuthUser authUser)
        {
            var usernameExists = await _dataContext.AuthUsers.AnyAsync(u => u.Username == authUser.Username);
            if (usernameExists) throw new InvalidOperationException($"Username is taken");
            var emailExists = await _dataContext.AuthUsers.AnyAsync(u => u.Email == authUser.Email);
            if (emailExists) throw new InvalidOperationException("Account with this email already exists");

            await _dataContext.AuthUsers.AddAsync(authUser);

            try
            {
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation("User registration successful.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while registering the user.");
                throw;
            }
            
            return authUser;
        }
    }
}