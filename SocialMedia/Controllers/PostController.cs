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
        public async Task<ActionResult<PostResponseDTO>> Get([FromBody] PostsFilterDTO filters)
        {


            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty) {
                return Unauthorized();
            }


            var postList = await _postService.GetAllPostsAsync(filters, Guid.Parse(LoggedInUser));
            return Ok(postList);
        }

        [HttpPost("/CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO newPost)
        {
            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty)
            {
                return Unauthorized();
            }

            Guid UserId = Guid.Parse(LoggedInUser);

            PostResponseDTO createdPost = await _postService.CreatePostAsync(newPost,UserId);
            return CreatedAtAction(
                    nameof(GetPostByID),
                    createdPost
                );
        }

        [HttpPost]
        [Route("React")]
        public async Task<IActionResult> ReactToPost([FromBody] ReactToPostDTO Reaction)
        {

            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty)
            {
                return Unauthorized();
            }

            var UserId = Guid.Parse(LoggedInUser);

            await _postService.PostReaction(Reaction,UserId);
            
            return NoContent();
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetPostByID([FromRoute] Guid Id)
        {
            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty)
            {
                return Unauthorized();
            }


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

            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty)
            {
                return Unauthorized();
            }

         

            PostResponseDTO? post = await _postService.GetPostByIdAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            if(post.UserId.ToString() != LoggedInUser) 
            {
                return Forbid();
            }

         
            await _postService.DeletePostAsync(Id);
            return NoContent();
        }


        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, [FromBody] CreatePostDTO UpdatedPost)
        {

            string LoggedInUser = await _postService.VerifyUser(User);

            if (LoggedInUser == string.Empty)
            {
                return Unauthorized();
            }

         

            PostResponseDTO? post = await _postService.GetPostByIdAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId.ToString() != LoggedInUser)
            {
                return Forbid();
            }


            await _postService.UpdatePostAsync(Id, UpdatedPost);
            return NoContent();
        }
    }
}
