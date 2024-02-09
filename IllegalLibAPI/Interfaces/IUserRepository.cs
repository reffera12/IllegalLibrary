using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> UpdateUserAsync(Guid userId, User user);
        Task DeleteUserAsync(Guid userId);
        Task ChangePasswordAsync(User user, string newPassword, bool hashedPassword = false);
    }
}