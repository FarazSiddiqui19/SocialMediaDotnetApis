using Microsoft.AspNetCore.Mvc;
using SocialMedia.models;
using SocialMedia.models.DTO.PostReaction;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("reaction")]
    public class PostReactionController : ControllerBase
    {
        private readonly IPostReactionService _service;

        public PostReactionController(IPostReactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> React(
            [FromBody] ReactToPostDTO dto
            )
        {
           

            await _service.ToggleReactionAsync(dto);
            return Ok();
        }

        
    }

}
