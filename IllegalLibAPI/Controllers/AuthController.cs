using IllegalLibAPI.Data;
using IllegalLibAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IllegalLibAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly JwtTokenService _jwtTokenService;
        private readonly TokenGenerator _tokenGenerator;
        private readonly DataContext _dataContext;

        public AuthController(AuthService authService, ILogger<AuthController> logger, JwtTokenService jwtTokenService, TokenGenerator tokenGenerator, DataContext dataContext)
        {
            _authService = authService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
            _tokenGenerator = tokenGenerator;
            _dataContext = dataContext;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var result = await _authService.AuthenticateAsync(username, password);

            if (result == null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterUserAsync(registerDTO);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok("User registered successfully" + '\n' + result);

        }
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(string accessToken, string refreshToken)
        {
            if (accessToken == null || refreshToken == null) return BadRequest("Invalid client request");

            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            if (username == null)
                return BadRequest("Invalid access token");

            var user = _dataContext.AuthUsers.FirstOrDefault(a => a.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _jwtTokenService.Authenticate(username);
            var newRefreshToken = _tokenGenerator.GenerateResetOrRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _dataContext.SaveChangesAsync();

            return Ok((newAccessToken, newRefreshToken));
        }

        [HttpPost]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return Ok();
        }
    }
}