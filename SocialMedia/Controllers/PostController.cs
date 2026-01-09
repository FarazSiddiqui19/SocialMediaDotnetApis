using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.models;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
        

            var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            
            Console.WriteLine($"UserId from token: {UserIdClaim.Value}");
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
            PostResponseDTO? post = await _postService.GetPostByIdAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            string? creatorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
           
            if (creatorId == null || post.UserId.ToString() != creatorId)
            {
                return Forbid();
            }
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
