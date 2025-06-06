using NUnit.Framework;
using Projekt;
using System;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture]
public class NbpServiceTests
{
    private NbpService? _service;

    [SetUp]
    public void SetUp()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.nbp.pl/api/") // poprawny adres z https
        };
        _service = new NbpService(httpClient);
    }

    [Test]
    public async Task GetExchangeRate_ReturnsPositiveValue()
    {
        Assert.That(_service, Is.Not.Null);

        var rate = await _service!.GetExchangeRate("USD");

        Assert.That(rate, Is.GreaterThan(0), "Kurs powinien być większy od zera");
    }
}
