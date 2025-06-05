using Projekt.Models;

namespace Projekt.Services
{
    public class DogService
    {
        private readonly HttpClient _httpClient;

        public DogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DogResponse?> GetRandomDogAsync()
        {
            return await _httpClient.GetFromJsonAsync<DogResponse>("breeds/image/random");
        }
    }
}
