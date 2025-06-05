using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]  // tylko Admin może wywołać endpointy w tym kontrolerze
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
