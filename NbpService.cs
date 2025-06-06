using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;   // Umożliwia bezpośrednie pobieranie i deserializację JSON do obiektów
using System.Threading.Tasks;
using System;
using System.Linq;           // Potrzebne do metod LINQ, takich jak .Any() i .First()

namespace Projekt;

public class NbpService
{
    private readonly HttpClient _httpClient;  // HttpClient do wykonywania zapytań HTTP

    // Konstruktor wstrzykuje HttpClient, skonfigurowany np. w Program.cs (AddHttpClient)
    public NbpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // BaseAddress jest już ustawiony w Program.cs podczas konfiguracji HttpClient
    }

    // Metoda asynchroniczna pobierająca aktualne kursy walut z API NBP
    // Parametr tableType oznacza typ tabeli kursów (domyślnie "A" - najpopularniejsze waluty)
    public async Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(string tableType = "A")
    {
        try
        {
            // Pobieramy listę tabel kursów w formacie JSON
            // Endpoint zwraca listę tabel, dlatego odbieramy List<ExchangeRateTable>
            var response = await _httpClient.GetFromJsonAsync<List<ExchangeRateTable>>(
                $"exchangerates/tables/{tableType}/?format=json");

            if (response != null && response.Any())
            {
                // Zwracamy listę kursów z pierwszej (najnowszej) tabeli
                return response.First().Rates;
            }
            // Jeśli odpowiedź pusta, zwracamy pustą kolekcję
            return Enumerable.Empty<ExchangeRate>();
        }
        catch (HttpRequestException e)
        {
            // Obsługa błędów sieciowych (np. brak internetu, błąd serwera)
            Console.WriteLine($"Błąd podczas pobierania danych z NBP: {e.Message}");
            return Enumerable.Empty<ExchangeRate>();
        }
        catch (NotSupportedException) // Może się pojawić przy problemach z deserializacją JSON
        {
            Console.WriteLine("Błąd podczas deserializacji JSON. Sprawdź format danych z API.");
            return Enumerable.Empty<ExchangeRate>();
        }
    }

    // Metoda zwracająca kurs średni (Mid) dla podanego kodu waluty
    public async Task<decimal> GetExchangeRate(string currencyCode)
    {
        // Pobieramy aktualne kursy (domyślnie tabela "A")
        var rates = await GetCurrentExchangeRatesAsync();

        // Szukamy kursu o podanym kodzie waluty (ignorując wielkość liter)
        var rate = rates.FirstOrDefault(r => r.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));

        // Jeśli nie znaleziono waluty, rzucamy wyjątek
        if (rate == null)
            throw new ArgumentException($"Waluta {currencyCode} nie znaleziona.");

        // Zwracamy kurs średni waluty
        return rate.Mid;
    }
}
