using System.Collections.Generic;
using System.Threading.Tasks;
using Projekt.Models;

namespace Projekt.Services
{
    /// <summary>
    /// Interfejs definiujący metody komunikacji z zewnętrznym API JSONPlaceholder.
    /// Służy do zarządzania postami (CRUD).
    /// </summary>
    public interface IJsonPlaceholderService
    {
        /// <summary>
        /// Pobiera listę wszystkich postów.
        /// </summary>
        /// <returns>Kolekcja obiektów Post</returns>
        Task<IEnumerable<Post>> GetPostsAsync();

        /// <summary>
        /// Pobiera jeden post na podstawie identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator posta</param>
        /// <returns>Obiekt Post lub null, jeśli nie znaleziono</returns>
        Task<Post> GetPostAsync(int id);

        /// <summary>
        /// Tworzy nowy post w zewnętrznym API.
        /// </summary>
        /// <param name="post">Dane nowego posta</param>
        /// <returns>Utworzony obiekt Post z nadanym ID</returns>
        Task<Post> CreatePostAsync(Post post);

        /// <summary>
        /// Aktualizuje istniejący post.
        /// </summary>
        /// <param name="id">Identyfikator posta do aktualizacji</param>
        /// <param name="post">Zaktualizowane dane posta</param>
        /// <returns>Zaktualizowany obiekt Post lub null, jeśli nie znaleziono</returns>
        Task<Post> UpdatePostAsync(int id, Post post);

        /// <summary>
        /// Usuwa post na podstawie identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator posta do usunięcia</param>
        /// <returns>True, jeśli operacja się powiodła; false w przeciwnym razie</returns>
        Task<bool> DeletePostAsync(int id);
    }
}
