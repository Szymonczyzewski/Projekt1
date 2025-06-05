using Microsoft.AspNetCore.Mvc;
using Projekt.Auth;
using System.ComponentModel.DataAnnotations;

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

        // DTO do logowania z walidacją
        public class LoginRequest
        {
            [Required(ErrorMessage = "Username jest wymagany")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password jest wymagane")]
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Sprawdzamy walidację modelu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
