using Microsoft.AspNetCore.Mvc;
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
    }
}
