using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;              // Potrzebne do filtrowania kolekcji przez LINQ(składnia i zestaw bibliotek w .NET)
using System.Threading.Tasks;
using Projekt;                  // Zawiera definicję NbpService i modelu ExchangeRate

namespace Projekt.Controllers
{
    [ApiController]              // Oznacza to Web API, włącza m.in. automatyczną walidację modelu
    [Route("[controller]")]      // Ścieżka będzie /ExchangeRates (nazwa klasy bez sufiksu "Controller")
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly NbpService _nbpService;

        // Dependency Injection: logger i serwis do pobierania kursów z NBP
        public ExchangeRatesController(ILogger<ExchangeRatesController> logger, NbpService nbpService)
        {
            _logger = logger;           // Do zapisywania komunikatów w logach
            _nbpService = nbpService;   // Do wywoływania API NBP
        }

        [HttpGet]                                // Odpowiada na GET /ExchangeRates
        public async Task<IEnumerable<ExchangeRate>> Get()
        {
            _logger.LogInformation("Pobieranie kursów walut z NBP.");
            // Pobieranie wszystkich kursów z zewnętrznego serwisu
            var rates = await _nbpService.GetCurrentExchangeRatesAsync();

            // Tylko wybrane waluty (bez RUB, bo może być niedostępny)
            var wantedCodes = new[] { "USD", "GBP", "CZK", "EUR", "JPY" };

            // Filtrowanie po kodzie waluty; LINQ wykona zapytanie w pamięci na otrzymanej liście
            var filtered = rates
                .Where(r => wantedCodes.Contains(r.Code))
                .ToList();

            // Zwrócenie JSON-a z listą tylko interesujących nas kursów
            return filtered;
        }
    }
}