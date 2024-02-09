using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthUser> AuthenticateUserAsync(AuthUser authUser, bool hashedPassword = false);
        Task<AuthUser> RegisterUserAsync(AuthUser authUser);
        Task RecoverPasswordAsync(string email);
    }
}