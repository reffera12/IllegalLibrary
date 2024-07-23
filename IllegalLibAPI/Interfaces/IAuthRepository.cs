using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> AuthenticateUserAsync(string username, string password);
        Task<AuthUser> RegisterUserAsync(RegisterDTO userToRegister);
        Task RecoverPasswordAsync(string email, string token, string newPassword);
        Task<string> AssignResetTokenAsync(string email);
        Task<AuthUser> GetUserByEmailAsync(string email);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsEmailTakenAsync(string email);
    }
}