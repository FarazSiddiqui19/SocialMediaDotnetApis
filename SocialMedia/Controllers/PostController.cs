using Microsoft.AspNetCore.Mvc;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.PostReaction;
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

        [HttpPost]
        [Route("GetPostsList")]
        public async Task<ActionResult<PostResponseDTO>> Get([FromBody]PostsFilterDTO filters,Guid? UserId)
        {
            var postList = await _postService.GetAllPostsAsync(filters, UserId);
            return Ok(postList);
        }

        [HttpPost("/CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
        {
            var createdPost = await _postService.CreatePostAsync(postDto);
            return CreatedAtAction(
                nameof(GetPostByID),
                new { PostId = createdPost.Id },
                createdPost
            );
        }

        [HttpPost]
        [Route("React")]
        public async Task<IActionResult> ReactToPost([FromBody] ReactToPostDTO Reaction)
        {
           await _postService.PostReaction(Reaction);
            
            return NoContent();
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetPostByID([FromRoute] Guid Id)
        {
            var post = await _postService.GetPostByIdAsync(Id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid Id)
        {
            await _postService.DeletePostAsync(Id);
            return NoContent();
        }


        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, [FromBody] CreatePostDTO UpdatedPost)
        {
            var post = await _postService.GetPostByIdAsync(Id);
            if (post == null)
            {
                return NotFound();
            }
            await _postService.UpdatePostAsync(Id, UpdatedPost);
            return NoContent();
        }
    }
}
