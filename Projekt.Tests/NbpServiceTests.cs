using NUnit.Framework; // Framework testowy NUnit
using Projekt;          // Importujemy przestrzeń nazw, gdzie jest NbpService
using System;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture] // Oznacza, że ta klasa zawiera testy jednostkowe
public class NbpServiceTests
{
    // Pole przechowujące instancję testowanej klasy NbpService
    private NbpService? _service;

    [SetUp] // Metoda uruchamiana przed każdym testem - przygotowanie środowiska
    public void SetUp()
    {
        // Tworzymy klienta HTTP z bazowym adresem API NBP
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.nbp.pl/api/") // Ustawiamy bazowy adres do wywołań
        };

        // Tworzymy instancję NbpService z powyższym HttpClientem
        _service = new NbpService(httpClient);
    }

    [Test] // Definiuje pojedynczy test
    public async Task GetExchangeRate_ReturnsPositiveValue()
    {
        // Sprawdzamy, czy _service zostało poprawnie utworzone (nie jest null)
        Assert.That(_service, Is.Not.Null);

        // Wywołujemy asynchroniczną metodę pobierającą kurs wymiany dla USD
        var rate = await _service!.GetExchangeRate("USD");

        // Sprawdzamy, czy otrzymany kurs jest większy od 0, co ma sens
        Assert.That(rate, Is.GreaterThan(0), "Kurs powinien być większy od zera");
    }
}
