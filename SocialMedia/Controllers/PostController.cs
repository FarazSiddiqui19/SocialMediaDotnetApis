using Microsoft.AspNetCore.Mvc;
using SocialMedia.models.DTO.Posts;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
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
        public async Task<IActionResult> CreatePost(AddPostsDTO postDto)
        {
            var createdPost = await _postService.CreatePostAsync(postDto);
            return Created();
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPostByID(Guid PostId)
        {
            var post = await _postService.GetPostByIdAsync(PostId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost("id")]
        public async Task<IActionResult> DeletePost(Guid PostId)
        {
            await _postService.DeletePostAsync(PostId);
            return Ok();
        }
    }
}
