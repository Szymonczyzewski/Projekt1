// Przestrzeń nazw projektu – organizuje klasy w logiczne grupy
namespace Projekt.Models
{
    // Klasa reprezentuje odpowiedź z zewnętrznego API z losowym psem
    public class DogResponse
    {
        // Właściwość przechowuje link do obrazka psa (lub komunikat)
        // "?" oznacza, że wartość może być null (czyli nieobecna)
        public string? Message { get; set; }

        // Właściwość przechowuje status odpowiedzi, np. "success"
        public string? Status { get; set; }
    }
}
