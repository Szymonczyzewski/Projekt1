// Importujemy potrzebne przestrzenie nazw:
// - ASP.NET Core do kontrolera
// - Modele danych
// - Serwisy do komunikacji z API zewnętrznym (jsonplaceholder.typicode.com)
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using Projekt.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    // To jest kontroler API (działa z JSON, walidacją itd.)
    [ApiController]

    // Ustawiamy ścieżkę do endpointów jako: /api/jsonplaceholder
    [Route("api/[controller]")]
    public class JsonPlaceholderController : ControllerBase
    {
        // Serwis do obsługi żądań HTTP do zewnętrznego API jsonplaceholder
        private readonly IJsonPlaceholderService _jsonPlaceholderService;

        // Wstrzykujemy serwis przez konstruktor (Dependency Injection)
        public JsonPlaceholderController(IJsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }

        // Endpoint GET: /api/jsonplaceholder/posts
        // Pobiera wszystkie posty z zewnętrznego API
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _jsonPlaceholderService.GetPostsAsync();
            return Ok(posts); // Zwraca status 200 OK z listą postów
        }

        // Endpoint GET: /api/jsonplaceholder/posts/{id}
        // Pobiera pojedynczy post o konkretnym ID
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _jsonPlaceholderService.GetPostAsync(id);

            // Jeśli post nie istnieje – zwracamy 404
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post); // Zwracamy 200 OK z danymi posta
        }

        // Endpoint POST: /api/jsonplaceholder/posts
        // Tworzy nowy post – wysyłany w treści żądania jako JSON
        [HttpPost("posts")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            var createdPost = await _jsonPlaceholderService.CreatePostAsync(post);

            // Zwracamy 201 Created z linkiem do nowo utworzonego zasobu
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }

        // Endpoint PUT: /api/jsonplaceholder/posts/{id}
        // Aktualizuje istniejący post o podanym ID
        [HttpPut("posts/{id}")]
        public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post post)
        {
            var updatedPost = await _jsonPlaceholderService.UpdatePostAsync(id, post);

            // Jeśli nie znaleziono posta do aktualizacji – zwracamy 404
            if (updatedPost == null)
            {
                return NotFound();
            }

            // W przeciwnym razie – zwracamy 200 OK z nowymi danymi
            return Ok(updatedPost);
        }

        // Endpoint DELETE: /api/jsonplaceholder/posts/{id}
        // Usuwa post o podanym ID
        [HttpDelete("posts/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var success = await _jsonPlaceholderService.DeletePostAsync(id);

            // Jeśli nie udało się usunąć (np. brak posta) – 404
            if (!success)
            {
                return NotFound();
            }

            // Zwracamy 204 No Content – usunięto poprawnie, ale bez zwracania danych
            return NoContent();
        }
    }
}
