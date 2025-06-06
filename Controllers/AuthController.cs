// Importujemy przestrzenie nazw potrzebne do działania kontrolera, autoryzacji i walidacji
using Microsoft.AspNetCore.Mvc;
using Projekt.Auth;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Auth
{
    // Deklarujemy, że to kontroler API (automatycznie obsługuje JSON, błędy itd.)
    [ApiController]

    // Adres URL tego kontrolera to "api/auth"
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Serwis odpowiedzialny za generowanie tokenów JWT
        private readonly AuthService _authService;

        // Wstrzykiwanie zależności przez konstruktor – otrzymujemy instancję AuthService
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Klasa pomocnicza (Data Transfer Object), która reprezentuje dane wejściowe przy logowaniu
        // Zawiera walidację pól (atrybuty [Required])
        public class LoginRequest
        {
            [Required(ErrorMessage = "Username jest wymagany")] // Wymagane pole
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password jest wymagane")] // Wymagane pole
            public string Password { get; set; } = string.Empty;
        }

        // Endpoint POST: api/auth/login
        // Przyjmuje dane logowania i zwraca token JWT, jeśli dane są poprawne
        [HttpPost("login")]

        // Opis możliwych odpowiedzi HTTP
        [ProducesResponseType(StatusCodes.Status200OK)]         // Sukces
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Błąd walidacji
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Nieprawidłowe dane

        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Sprawdzamy, czy dane logowania spełniają wymagania walidacji
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Zwracamy błąd 400 z opisem problemu
            }

            // Uproszczone logowanie – sprawdzamy, czy hasło to dokładnie "admin"
            // W praktyce tu powinno być sprawdzanie z bazą danych i hasło zahashowane!
            if (request.Password != "admin")
            {
                return Unauthorized(new { message = "Nieprawidłowe hasło" }); // Zwracamy 401
            }

            // Generujemy token JWT dla podanego użytkownika
            var token = _authService.GenerateJwtToken(request.Username);

            // Zwracamy token w odpowiedzi 200 OK jako JSON
            return Ok(new
            {
                token = token
            });
        }
    }
}
