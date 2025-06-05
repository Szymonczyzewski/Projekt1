using Projekt.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Projekt.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("posts");
            return posts ?? new List<Post>();
        }

        public async Task<Post> GetPostAsync(int id)
        {
            var post = await _httpClient.GetFromJsonAsync<Post>($"posts/{id}");
            return post ?? throw new KeyNotFoundException($"Post with id {id} not found");
        }

        public async Task<Post> CreatePostAsync(Post newPost)
        {
            var response = await _httpClient.PostAsJsonAsync("posts", newPost);
            response.EnsureSuccessStatusCode();
            var createdPost = await response.Content.ReadFromJsonAsync<Post>();
            return createdPost!;
        }

        public async Task<Post> UpdatePostAsync(int id, Post updatedPost)
        {
            var response = await _httpClient.PutAsJsonAsync($"posts/{id}", updatedPost);
            response.EnsureSuccessStatusCode();
            var updated = await response.Content.ReadFromJsonAsync<Post>();
            return updated!;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"posts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
