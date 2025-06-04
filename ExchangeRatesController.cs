using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt; // Zawiera definicję NbpService i modelu ExchangeRate
using Microsoft.Extensions.Logging;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly NbpService _nbpService;

        public ExchangeRatesController(ILogger<ExchangeRatesController> logger, NbpService nbpService)
        {
            _logger = logger;
            _nbpService = nbpService;
        }

        [HttpGet]
        public async Task<IEnumerable<ExchangeRate>> Get()
        {
            _logger.LogInformation("Pobieranie kursów walut z NBP.");

            var rates = await _nbpService.GetCurrentExchangeRatesAsync();

            var wantedCodes = new[] { "USD", "GBP", "CZK", "EUR", "JPY" };

            var filtered = rates.Where(r => wantedCodes.Contains(r.Code)).ToList();

            return filtered;
        }
    }
}
