using Projekt.Models; // Importujemy model DogResponse, który odpowiada strukturze JSON z API

namespace Projekt.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za komunikację z API z losowymi zdjęciami psów.
    /// </summary>
    public class DogService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Konstruktor przyjmujący HttpClient, który powinien mieć ustawioną bazową ścieżkę URL.
        /// </summary>
        /// <param name="httpClient">HttpClient skonfigurowany do komunikacji z dog.ceo API</param>
        public DogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Wysyła żądanie do API i zwraca dane o losowym psie w postaci obiektu DogResponse.
        /// </summary>
        /// <returns>Obiekt DogResponse zawierający URL zdjęcia psa i status odpowiedzi; może być null w przypadku błędu</returns>
        public async Task<DogResponse?> GetRandomDogAsync()
        {
            // Wysyłamy żądanie GET do endpointu i automatycznie deserializujemy JSON do obiektu DogResponse
            return await _httpClient.GetFromJsonAsync<DogResponse>("breeds/image/random");
        }
    }
}
