using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data;
using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.DTO.Users;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;


namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersServices _usersService;
        private readonly IEmailVerificationService _emailVerificationService;
      
      

        public UserController(
            IUsersServices userServices,
            IEmailVerificationService emailVerificationService)
        {
            _usersService = userServices;
            _emailVerificationService = emailVerificationService;
           
          
        }

       

        [HttpGet("friend-requests")]
        [Authorize]
        public async Task<IActionResult> GetFriendRequests([FromQuery] RequestFilterDTO DTO)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();


            var result =  await _usersService.GetAllFriendRequests(userId.Value,DTO.PageSize,DTO.Page);
            return Ok(result);
        }

        [HttpPost("friend-requests/send/{receiverId:guid}")]
        [Authorize]
        public async Task<IActionResult> SendFriendRequest(Guid receiverId)
        {
            var senderId = GetCurrentUserId();
            if (senderId == null) return Unauthorized();

            await _usersService.SendFriendRequest(senderId.Value,receiverId);
          
            return Ok("Friend request sent.");
        }

        [HttpPut("friend-requests/respond/{SenderId:guid}")]
        [Authorize]
        public async Task<IActionResult> RespondToFriendRequest( Guid SenderId,Status status)
        {
            var recipientId = GetCurrentUserId();
            if(recipientId == null) return Unauthorized();
           

            await _usersService.RespondToFriendRequest(new FriendRequest
            {
                SenderId = SenderId,
                RecieverId = recipientId.Value,
                status = status
            });
           
            return NoContent();
        }

 

        [HttpPost]
        [Route("GetUsersList")]
        public async Task<IActionResult> Get([FromBody] UsersFilter filter)
        {
            PagedResults<UserResponseDto> userslist = await _usersService.GetAllUsersAsync(filter);
            return Ok(userslist);
        }

        [HttpPost]
        [Route("GetFriendList")]
        [Authorize]
        public async Task<IActionResult> GetFriendList([FromBody] UsersFilter filter)
        {
            var userId = GetCurrentUserId();
            //if (userId == null) return Unauthorized();
            //var friends = await _usersService.GetUserFriendListAsync(userId.Value);
            return Ok();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO user)
        {
            bool checkemail = await _emailVerificationService.GetDataAsync(user.Email);

            if (checkemail == false)
            {
                return BadRequest("Email is Invalid");
            }

            UserResponseDto createdUser = await _usersService.CreateUserAsync(user);

            return CreatedAtAction(
                  nameof(GetUserByID),
                  new { id = createdUser.Id },
                  createdUser
            );
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO UserLoginRequest)
        {

            UserLoginResposeDTO? user = await _usersService.LoginAsync(UserLoginRequest);

            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetUserByID([FromRoute] Guid Id)
        {
            UserResponseDto? user = await _usersService.GetUserByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete]
        [Route("Id")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            await _usersService.DeleteUserAsync(userId.Value);
            return NoContent();
        }

        [HttpPut]
        [Route("Id")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] CreateUserDTO updatedUser)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var user = await _usersService.UpdateUserAsync(userId.Value, updatedUser);
            if (user == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        private Guid? GetCurrentUserId()
        {
            Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value,out Guid userId))
            {
                
                return null;
            }


            return userId;
        }
    }
}

