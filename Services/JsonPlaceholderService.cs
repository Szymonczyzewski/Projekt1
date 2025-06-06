using Projekt.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Projekt.Services
{
    /// <summary>
    /// Implementacja interfejsu IJsonPlaceholderService.
    /// Komunikuje się z zewnętrznym API JSONPlaceholder do zarządzania postami.
    /// </summary>
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Konstruktor z wstrzykiwaniem zależności HttpClient.
        /// </summary>
        /// <param name="httpClient">Instancja HttpClient ustawiona z bazowym adresem URL</param>
        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Pobiera wszystkie posty z API.
        /// </summary>
        /// <returns>Lista postów lub pusta lista, jeśli nic nie zwrócono</returns>
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("posts");
            return posts ?? new List<Post>();
        }

        /// <summary>
        /// Pobiera jeden post na podstawie ID.
        /// </summary>
        /// <param name="id">Identyfikator posta</param>
        /// <returns>Obiekt Post, jeśli istnieje</returns>
        /// <exception cref="KeyNotFoundException">Rzucany, gdy post nie istnieje</exception>
        public async Task<Post> GetPostAsync(int id)
        {
            var post = await _httpClient.GetFromJsonAsync<Post>($"posts/{id}");
            return post ?? throw new KeyNotFoundException($"Post with id {id} not found");
        }

        /// <summary>
        /// Tworzy nowy post poprzez wysłanie danych metodą POST.
        /// </summary>
        /// <param name="newPost">Dane nowego posta</param>
        /// <returns>Utworzony post z nadanym ID</returns>
        public async Task<Post> CreatePostAsync(Post newPost)
        {
            var response = await _httpClient.PostAsJsonAsync("posts", newPost);
            response.EnsureSuccessStatusCode(); // Rzuca wyjątek, jeśli odpowiedź nie ma statusu 2xx
            var createdPost = await response.Content.ReadFromJsonAsync<Post>();
            return createdPost!;
        }

        /// <summary>
        /// Aktualizuje istniejący post za pomocą metody PUT.
        /// </summary>
        /// <param name="id">ID posta do zaktualizowania</param>
        /// <param name="updatedPost">Nowe dane posta</param>
        /// <returns>Zaktualizowany post</returns>
        public async Task<Post> UpdatePostAsync(int id, Post updatedPost)
        {
            var response = await _httpClient.PutAsJsonAsync($"posts/{id}", updatedPost);
            response.EnsureSuccessStatusCode();
            var updated = await response.Content.ReadFromJsonAsync<Post>();
            return updated!;
        }

        /// <summary>
        /// Usuwa post na podstawie ID.
        /// </summary>
        /// <param name="id">ID posta do usunięcia</param>
        /// <returns>True, jeśli usunięcie zakończyło się sukcesem</returns>
        public async Task<bool> DeletePostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"posts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
