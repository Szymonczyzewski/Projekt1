using System.Threading.Tasks;

namespace Projekt.Services
{
    /// <summary>
    /// Usługa symulująca wykonanie płatności przy użyciu API PayU.
    /// W docelowej wersji projektu tutaj znajdzie się faktyczne wywołanie API PayU.
    /// </summary>
    public class PayuService
    {
        /// <summary>
        /// Symuluje wykonanie płatności.
        /// W pełnej wersji tutaj powinno zostać zaimplementowane rzeczywiste wywołanie API PayU.
        /// </summary>
        /// <param name="amount">Kwota do zapłaty</param>
        /// <param name="description">Opis płatności</param>
        /// <returns>Wiadomość potwierdzająca symulowaną płatność</returns>
        public Task<string> MakePaymentAsync(decimal amount, string description)
        {
            // Zwraca od razu wynik jako Task, bez rzeczywistej komunikacji z API
            return Task.FromResult($"Symulowana płatność {amount} zł: {description}");
        }
    }
}
