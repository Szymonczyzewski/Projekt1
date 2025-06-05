using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // Dostępne tylko dla roli "Admin"
        [Authorize(Roles = "Admin")]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            return Ok("Tylko admin ma dostęp do tej tajnej informacji 🔐");
        }

        // Dostępne dla wszystkich zalogowanych
        [Authorize]
        [HttpGet("public")]
        public IActionResult GetPublic()
        {
            return Ok("Każdy zalogowany użytkownik widzi to info ✅");
        }
    }
}
