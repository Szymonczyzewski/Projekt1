// Importujemy wymagane przestrzenie nazw do autoryzacji i obsługi kontrolera
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projekt.Controllers
{
    // Atrybut informuje, że to kontroler API (automatycznie obsługuje błędy i format JSON)
    [ApiController]

    // Ustalony adres URL: "api/admin" (nazwa kontrolera bez "Controller")
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // Endpoint dostępny tylko dla użytkowników z rolą "Admin"
        [Authorize(Roles = "Admin")] // Sprawdza, czy zalogowany użytkownik ma przypisaną rolę "Admin"
        [HttpGet("secret")]          // GET: api/admin/secret
        public IActionResult GetSecret()
        {
            // Zwracamy wiadomość tekstową jako odpowiedź 200 OK
            return Ok("Tylko admin ma dostęp do tej tajnej informacji 🔐");
        }

        // Endpoint dostępny dla każdego zalogowanego użytkownika (rola nie ma znaczenia)
        [Authorize]                 // Wymaga tylko, żeby użytkownik był zalogowany (miał ważny token JWT)
        [HttpGet("public")]         // GET: api/admin/public
        public IActionResult GetPublic()
        {
            // Zwracamy prostą wiadomość jako odpowiedź 200 OK
            return Ok("Każdy zalogowany użytkownik widzi to info ✅");
        }
    }
}
