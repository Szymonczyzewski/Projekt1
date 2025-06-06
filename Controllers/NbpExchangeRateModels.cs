// Używamy tej przestrzeni nazw, aby móc oznaczać pola JSON (np. JsonPropertyName)
using System.Text.Json.Serialization;

namespace Projekt // Przestrzeń nazw, która grupuje klasy – taka "kategoria" w projekcie
{
    // Model danych odpowiadający strukturze tabeli kursów walut z API NBP
    public class ExchangeRateTable
    {
        // "table" to typ tabeli np. "A" lub "C"
        [JsonPropertyName("table")]
        public string Table { get; set; } = string.Empty;

        // "no" to numer tabeli, np. "115/A/NBP/2024"
        [JsonPropertyName("no")]
        public string No { get; set; } = string.Empty;

        // "effectiveDate" to data publikacji kursów (bez godziny)
        [JsonPropertyName("effectiveDate")]
        public DateOnly EffectiveDate { get; set; }

        // "rates" to lista kursów walut – każdy to obiekt klasy ExchangeRate
        [JsonPropertyName("rates")]
        public List<ExchangeRate> Rates { get; set; } = new List<ExchangeRate>();
    }

    // Model danych pojedynczego kursu waluty
    public class ExchangeRate
    {
        // "currency" to pełna nazwa waluty, np. "euro"
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        // "code" to kod waluty, np. "EUR"
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        // "mid" to średni kurs tej waluty względem PLN (np. 4.37)
        [JsonPropertyName("mid")]
        public decimal Mid { get; set; }
    }
}
