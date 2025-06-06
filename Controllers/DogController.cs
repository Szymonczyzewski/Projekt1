// Importujemy potrzebne przestrzenie nazw:
// - Autoryzacja JWT
// - Obsługa kontrolerów API
// - Serwis DogService (logika pobierania zdjęć psów)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt.Services;

namespace Projekt.Controllers
{
    // Informujemy, że to kontroler API (obsługuje JSON, walidację itd.)
    [ApiController]

    // Ustalony adres URL: "api/dog"
    [Route("api/[controller]")]
    public class DogController : ControllerBase
    {
        // Prywatne pole na serwis do pobierania zdjęć psów
        private readonly DogService _dogService;

        // Wstrzykujemy serwis DogService przez konstruktor
        public DogController(DogService dogService)
        {
            _dogService = dogService;
        }

        // Endpoint GET: /api/dog/random
        // Dostępny publicznie – nie wymaga logowania
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomDog()
        {
            // Wywołujemy metodę z serwisu, która pobiera zdjęcie psa (z zewnętrznego API)
            var result = await _dogService.GetRandomDogAsync();

            // Jeśli wynik jest null – zwracamy błąd 500
            if (result == null)
                return StatusCode(500, new { error = "Nie udało się pobrać zdjęcia psa." });

            // Jeśli się udało – zwracamy wynik 200 OK z danymi
            return Ok(result);
        }

        // Endpoint GET: /api/dog/secure-random
        // Dostępny tylko dla zalogowanych użytkowników (token JWT wymagany)
        [Authorize]
        [HttpGet("secure-random")]
        public async Task<IActionResult> GetSecureRandomDog()
        {
            // Pobieramy zdjęcie psa jak wyżej
            var result = await _dogService.GetRandomDogAsync();

            // Obsługa błędu, jeśli API zewnętrzne nie zadziała
            if (result == null)
                return StatusCode(500, new { error = "Nie udało się pobrać zdjęcia psa." });

            // Zwracamy wynik – tylko jeśli użytkownik jest zalogowany
            return Ok(result);
        }
    }
}
