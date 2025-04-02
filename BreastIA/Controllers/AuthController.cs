using BreastIA.Services;
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


        //> [!NOTE] 
        /// personalice los mensajes para 
        /// 
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isValid = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (!isValid)
            {
                return Unauthorized("Email o password invalid.");
            }

            return Ok(new { Message = "Wait a second..." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
