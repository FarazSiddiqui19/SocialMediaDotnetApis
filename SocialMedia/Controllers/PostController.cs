using Microsoft.AspNetCore.Mvc;
using SocialMedia.models.DTO.Posts;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostsServices _postService;

        public PostController(IPostsServices postServices)
        {
            _postService = postServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var postList = await _postService.GetAllPostsAsync();
            return Ok(postList);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost( [FromBody]AddPostsDTO postDto)
        {
            var createdPost = await _postService.CreatePostAsync(postDto);
            return CreatedAtAction(
                nameof(GetPostByID),
                new { PostId = createdPost.PostId },
                createdPost
            );
        }

        [HttpGet("{PostId:guid}")]
        public async Task<IActionResult> GetPostByID([FromRoute]Guid PostId)
        {
            var post = await _postService.GetPostByIdAsync(PostId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpDelete("{PostId:guid}")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid PostId)
        {
            await _postService.DeletePostAsync(PostId);
            return NoContent();
        }


        [HttpPatch("{PostId:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid PostId)
        {
           
            return NoContent();
        }
    }
}
