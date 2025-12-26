using Microsoft.AspNetCore.Mvc;
using SocialMedia.models.DTO.PostReaction;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("reaction/{postId}")]
    public class PostReactionController : ControllerBase
    {
        private readonly IPostReactionService _service;

        public PostReactionController(IPostReactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> React(
            Guid postId,
            [FromBody] ReactToPostDTO dto)
        {
            if (postId != dto.PostId)
                return BadRequest("PostId mismatch");

            await _service.ToggleReactionAsync(dto);
            return Ok();
        }
    }

}
