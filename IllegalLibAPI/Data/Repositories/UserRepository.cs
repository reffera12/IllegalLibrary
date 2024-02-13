using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace IllegalLibAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DataContext dataContext, ILogger<UserRepository> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            _logger.LogInformation($"Getting a user by {userId.ToString()}");

            var userData = await _dataContext.Users
            .Include(u => u.bookRequests)
            .Include(u => u.AuthUser)
            .Where(u => u.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"User with id: {userId} does not exist");
            return userData!;
        }

        public async Task<User> CreateUserAsync(User user, Guid userId)
        {

            if (user == null) _logger.LogWarning("Received request with empty user data");
            user.UserId = userId;
            await _dataContext.Users.AddAsync(user);
            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the user");
                throw;
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(Guid userId, User user)
        {
            var userToUpdate = await GetUserByIdAsync(userId);

            if (userToUpdate is null) return null;

            var usernameExists = await _dataContext.Users
            .AnyAsync(u => u.AuthUser.Username == user.AuthUser.Username);
            if (usernameExists) throw new InvalidOperationException($"User with this username already exists");

            userToUpdate.AuthUser.Username = user.AuthUser.Username;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Bio = user.Bio;

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }

            return userToUpdate;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userToRemove = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new KeyNotFoundException($"User with id: {userId} does not exist");
            try
            {
                _dataContext.Users.Remove(userToRemove);

            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                throw;
            }
            await _dataContext.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(User user, string newPassword, bool hashedPassword = false)
        {
            // Validate the old password before proceeding
            var oldPasswordIsValid = hashedPassword
                ? user.AuthUser.Password == newPassword
                : BCrypt.Net.BCrypt.Verify(newPassword, user.AuthUser.Password);

            if (!oldPasswordIsValid)
            {
                throw new InvalidOperationException("Invalid old password provided");
            }

            // Update the user's password
            user.AuthUser.Password = hashedPassword ? newPassword : BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Save changes to the database
            await _dataContext.SaveChangesAsync();
        }
    }
}