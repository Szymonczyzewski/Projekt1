using Microsoft.AspNetCore.Mvc;
using Projekt.Services;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // UWAGA: tu tylko prosty login, bez bazy danych
            if (login.Username == "admin" && login.Password == "admin123")
            {
                var token = _authService.GenerateJwtToken(login.Username);
                return Ok(new { token });
            }

            return Unauthorized("Niepoprawne dane logowania.");
        }
    }

    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
