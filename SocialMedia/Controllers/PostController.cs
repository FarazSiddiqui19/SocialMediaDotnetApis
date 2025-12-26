using Microsoft.AspNetCore.Mvc;
using SocialMedia.models.DTO;
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
        public async Task<IActionResult> Get([FromQuery]string? Title,int page=2, int pagesize = 1, SortingOrder order=SortingOrder.Asc)
        {
            var postList = await _postService.GetAllPostsAsync(Title,page,pagesize, order);
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


        [HttpPut("{PostId:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid PostId,[FromBody] AddPostsDTO updt)
        {
           var post = await _postService.GetPostByIdAsync(PostId);
            if (post == null)
            {
                return NotFound();
            }
            await _postService.UpdatePostAsync(PostId, updt);
            return NoContent();
        }
    }
}
