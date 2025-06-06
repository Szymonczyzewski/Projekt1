using Microsoft.AspNetCore.Mvc;            // Atrybuty i klasy do tworzenia kontrolerów Web API
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt;                              // Import lokalnego namespace zawierającego m.in. serwis NBP i model ExchangeRate
using Microsoft.Extensions.Logging;        // Do logowania
using Microsoft.AspNetCore.Authorization;  // Do atrybutu [Authorize]

namespace Projekt.Controllers
{
    [ApiController]                         // Określa, że ten kontroler to Web API
    [Route("[controller]")]                 // Ustawia trasę URL jako /ExchangeRates
    [Authorize(Roles = "Admin")]            // Endpointy są dostępne tylko dla użytkowników z rolą "Admin"
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ILogger<ExchangeRatesController> _logger;  // Logger do zapisywania informacji w logach
        private readonly NbpService _nbpService;                    // Serwis do pobierania danych z API NBP

        // Konstruktor z wstrzykiwaniem zależności: logger i serwis NBP
        public ExchangeRatesController(ILogger<ExchangeRatesController> logger, NbpService nbpService)
        {
            _logger = logger;
            _nbpService = nbpService;
        }

        // Endpoint GET /ExchangeRates
        [HttpGet]
        public async Task<IEnumerable<ExchangeRate>> Get()
        {
            // Informacja do logów, że rozpoczęto pobieranie danych
            _logger.LogInformation("Pobieranie kursów walut z NBP.");

            // Pobranie listy kursów walut z zewnętrznego API NBP
            var rates = await _nbpService.GetCurrentExchangeRatesAsync();

            // Lista kodów walut, które nas interesują
            var wantedCodes = new[] { "USD", "GBP", "CZK", "EUR", "JPY" };

            // Filtrowanie pobranych kursów, aby zostawić tylko te z powyższej listy
            var filtered = rates.Where(r => wantedCodes.Contains(r.Code)).ToList();

            // Zwrócenie przefiltrowanej listy jako odpowiedzi HTTP
            return filtered;
        }
    }
}
