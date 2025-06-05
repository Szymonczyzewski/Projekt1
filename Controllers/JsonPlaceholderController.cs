using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using Projekt.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JsonPlaceholderController : ControllerBase
    {
        private readonly IJsonPlaceholderService _jsonPlaceholderService;

        public JsonPlaceholderController(IJsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }

        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _jsonPlaceholderService.GetPostsAsync();
            return Ok(posts);
        }

        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _jsonPlaceholderService.GetPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost("posts")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            var createdPost = await _jsonPlaceholderService.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("posts/{id}")]
        public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post post)
        {
            var updatedPost = await _jsonPlaceholderService.UpdatePostAsync(id, post);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(updatedPost);
        }

        [HttpDelete("posts/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var success = await _jsonPlaceholderService.DeletePostAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
