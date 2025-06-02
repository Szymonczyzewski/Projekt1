using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // Dodaj to
using System.Threading.Tasks; // Dodaj to
using Projekt; // Dodaj to, aby NbpService i modele walut były dostępne

namespace Projekt.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRatesController : ControllerBase // Zmieniono nazwę klasy
{
    private readonly ILogger<ExchangeRatesController> _logger; // Zmieniono typ loggera
    private readonly NbpService _nbpService; // Dodaj serwis NBP

    public ExchangeRatesController(ILogger<ExchangeRatesController> logger, NbpService nbpService) // Zmieniono parametry konstruktora
    {
        _logger = logger;
        _nbpService = nbpService; // Wstrzyknięcie serwisu NBP
    }

    [HttpGet]
    public async Task<IEnumerable<ExchangeRate>> Get() // Zmieniono typ zwracany i dodano async
    {
        _logger.LogInformation("Pobieranie kursów walut z NBP.");
        var rates = await _nbpService.GetCurrentExchangeRatesAsync(); // Wywołanie serwisu NBP
        
        // Ogranicz do 5 walut, jeśli chcesz, dla przykładu.
        // Jeśli API zwraca mniej niż 5, zwróci tyle, ile jest.
        return rates.Take(5).ToList(); 
    }
}