using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> CreateUserAsync(AuthUser registeredUser, string firstName, string lastName);
        Task<User> UpdateUserAsync(Guid userId, User user);
        Task ChangePasswordAsync(User user, string newPassword);
    }
}