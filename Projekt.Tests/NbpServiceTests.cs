using NUnit.Framework; // This is crucial for NUnit attributes and assertions
using Projekt;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture] // NUnit attribute for a test class
public class NbpServiceTests
{
    private NbpService _service;

    [SetUp]
    public void SetUp()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://api.nbp.pl/api/")
        };
        _service = new NbpService(httpClient);
    }
    [Test] // NUnit attribute for a test method
    public async Task GetExchangeRate_ReturnsPositiveValue()
    {
        var rate = await _service.GetExchangeRate("USD");

        // NUnit assertion style
        Assert.That(rate, Is.GreaterThan(0), "Kurs powinien być większy od zera");
    }
}