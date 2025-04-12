using BreastIA.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BreastIA.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Email or password is invalid.");
            }

            // Generar y devolver el token
            var token = await _authService.GenerateTokenAsync(user.IdUsers);

            return Ok(new
            {
                Message = "Login successful.",
                Token = token
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            await _authService.LogoutAsync(token);
            return Ok(new { Message = "Logged out successfully." });
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            var user = await _authService.ValidateTokenAsync(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            return Ok(new
            {
                UserId = user.IdUsers,
                FullName = user.FullName
            });
        }
    }
}
