using System.Text.Json.Serialization; // Potrzebne dla atrybutu JsonPropertyName

namespace Projekt; // Utrzymaj tę przestrzeń nazw, aby uniknąć problemów z istniejącymi odwołaniami

public class ExchangeRateTable
{
    [JsonPropertyName("table")]
    public string Table { get; set; } = string.Empty; // Dodano inicjalizację
    
    [JsonPropertyName("no")]
    public string No { get; set; } = string.Empty; // Dodano inicjalizację

    [JsonPropertyName("effectiveDate")]
    public DateOnly EffectiveDate { get; set; }

    [JsonPropertyName("rates")]
    public List<ExchangeRate> Rates { get; set; } = new List<ExchangeRate>(); // Dodano inicjalizację
}

public class ExchangeRate
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty; // Dodano inicjalizację

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty; // Dodano inicjalizację

    [JsonPropertyName("mid")]
    public decimal Mid { get; set; }
}