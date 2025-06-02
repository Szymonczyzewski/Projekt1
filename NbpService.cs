using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Wymaga pakietu NuGet System.Net.Http.Json
using System.Threading.Tasks;
using System;
using System.Linq; // Potrzebne dla .Any() i .First()

namespace Projekt;

public class NbpService
{
    private readonly HttpClient _httpClient;

    public NbpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // BaseAddress jest już ustawiony w Program.cs za pomocą AddHttpClient
    }

    public async Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(string tableType = "A")
    {
        try
        {
            // Przykład dla tabeli A (najpopularniejsze waluty)
            // Możesz zmienić na "B" lub "C" w zależności od potrzeb
            var response = await _httpClient.GetFromJsonAsync<List<ExchangeRateTable>>($"exchangerates/tables/{tableType}/?format=json");

            if (response != null && response.Any())
            {
                // Zwracamy listę wszystkich kursów z pierwszej tabeli (najnowszej)
                return response.First().Rates;
            }
            return Enumerable.Empty<ExchangeRate>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Błąd podczas pobierania danych z NBP: {e.Message}");
            return Enumerable.Empty<ExchangeRate>();
        }
        catch (NotSupportedException) // Przykład błędu deserializacji
        {
            Console.WriteLine("Błąd podczas deserializacji JSON. Sprawdź format danych z API.");
            return Enumerable.Empty<ExchangeRate>();
        }
    }
}