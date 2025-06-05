using System.Collections.Generic;
using System.Threading.Tasks;
using Projekt.Models;

namespace Projekt.Services
{
    public interface IJsonPlaceholderService
    {
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<Post> GetPostAsync(int id);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> UpdatePostAsync(int id, Post post);
        Task<bool> DeletePostAsync(int id);
    }
}
