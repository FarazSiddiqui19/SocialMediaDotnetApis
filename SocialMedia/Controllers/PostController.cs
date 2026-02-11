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
            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);
            
            if (LoggedInUser == Guid.Empty) {
                return Unauthorized();
            }


            var postList = await _postService.GetAllPostsAsync(filters, LoggedInUser);
            return Ok(postList);
        }

        [HttpPost("/CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO newPost)
        {
            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);

            if (LoggedInUser == Guid.Empty)
            {
                return Unauthorized();
            }

           

            PostResponseDTO createdPost = await _postService.CreatePostAsync(newPost, LoggedInUser);
            return CreatedAtAction(
                    nameof(GetPostByID),
                    createdPost
                );
        }

        [HttpPost]
        [Route("React")]
        public async Task<IActionResult> ReactToPost([FromBody] ReactToPostDTO Reaction)
        {

            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);

            if (LoggedInUser == Guid.Empty)
            {
                return Unauthorized();
            }

            await _postService.PostReaction(Reaction, LoggedInUser);
            
            return NoContent();
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetPostByID([FromRoute] Guid Id)
        {
            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);

            if (LoggedInUser == Guid.Empty)
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

            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);

            if (LoggedInUser == Guid.Empty)
            {
                return Unauthorized();
            }



            PostResponseDTO? post = await _postService.GetPostByIdAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            if(post.UserId != LoggedInUser) 
            {
                return Forbid();
            }

         
            await _postService.DeletePostAsync(Id);
            return NoContent();
        }


        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, [FromBody] CreatePostDTO UpdatedPost)
        {
            Guid LoggedInUser = Guid.Empty;

            GetCurrentUser(LoggedInUser);

            if (LoggedInUser == Guid.Empty)
            {
                return Unauthorized();
            }




            PostResponseDTO? post = await _postService.GetPostByIdAsync(Id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId != LoggedInUser)
            {
                return Forbid();
            }


            await _postService.UpdatePostAsync(Id, UpdatedPost);
            return NoContent();
        }



        private void GetCurrentUser(Guid LoggedInUser)
        {
            Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdClaim.Value, out LoggedInUser);
           
        }
    }
}
