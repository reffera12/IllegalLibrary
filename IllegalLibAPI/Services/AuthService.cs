using IllegalLibAPI.Interfaces;
using IllegalLibAPI.Models;
using IllegalLibAPI.Data;
using System.Security.Authentication;

namespace IllegalLibAPI.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly DataContext _dataContext;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, DataContext dataContext, ILogger<AuthService> logger, JwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _dataContext = dataContext;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            try
            {
                var signInCredentials = await _authRepository.AuthenticateUserAsync(username, password);

                return signInCredentials;
            }
            catch (AuthenticationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating the user.");
                throw new AuthenticationException("Authentication failed.");
            }
        }
        public async Task<RegisterDTO> RegisterUserAsync(RegisterDTO userToRegister)
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
            return userToRegister;
        }
    }
}