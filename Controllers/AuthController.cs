using Microsoft.AspNetCore.Mvc;
using Projekt.Auth;

namespace Projekt.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // DTO do logowania
        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Uproszczona weryfikacja hasła - tylko "admin"
            if (request.Password != "admin")
            {
                return Unauthorized(new { message = "Nieprawidłowe hasło" });
            }

            var token = _authService.GenerateJwtToken(request.Username);

            return Ok(new
            {
                token = token
            });
        }
    }
}
