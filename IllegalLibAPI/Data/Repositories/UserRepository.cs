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
            var user = await _dataContext.Set<AuthUser>().Where(u => u.UserId == userId).FirstOrDefaultAsync();

            if (user == null || user.User == null)
            {
                throw new ArgumentException(userId + "does not exist");
            }
            return user.User;
        }

        public async Task<User> CreateUserAsync(AuthUser registeredUser, string firstName, string lastName)
        {
            if (firstName == string.Empty || lastName == string.Empty || registeredUser == null)
            {
                _logger.LogWarning("Received a request with invalid data");
                throw new ArgumentException("firstName and lastName cannot be null");
            }
            User userToCreate = new(registeredUser.UserId, firstName, lastName, registeredUser);

            await _dataContext.Users.AddAsync(userToCreate);
            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the user");
                throw;
            }
            return userToCreate;
        }

        public async Task<User> UpdateUserAsync(Guid userId, User user)
        {
            var userToUpdate = await GetUserByIdAsync(userId);

            if (userToUpdate is null) return null;

            var usernameExists = await _dataContext.Users
            .AnyAsync(u => u.AuthUser.Username == user.AuthUser.Username);
            if (usernameExists) throw new InvalidOperationException($"User with this username already exists");

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Bio = user.Bio;
            userToUpdate.AuthUser.Email = user.AuthUser.Email;

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