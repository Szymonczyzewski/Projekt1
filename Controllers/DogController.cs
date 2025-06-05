using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt.Services;

namespace Projekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogController : ControllerBase
    {
        private readonly DogService _dogService;

        public DogController(DogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomDog()
        {
            var result = await _dogService.GetRandomDogAsync();
            if (result == null)
                return StatusCode(500, new { error = "Nie udało się pobrać zdjęcia psa." });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("secure-random")]
        public async Task<IActionResult> GetSecureRandomDog()
        {
            var result = await _dogService.GetRandomDogAsync();
            if (result == null)
                return StatusCode(500, new { error = "Nie udało się pobrać zdjęcia psa." });

            return Ok(result);
        }
    }
}
