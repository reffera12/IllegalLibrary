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

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
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
            return Ok("User registered successfully" +'\n' + result);

        }
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken()
        {
            return Ok();
        }
        [HttpPost]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return Ok();
        }
    }
}