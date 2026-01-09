using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;


namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersServices _usersService;
        public UserController(IUsersServices UserServices)
        {
            _usersService = UserServices;
        }



        [HttpPost]
        [Route("GetUsersList")]

        public async Task<IActionResult> Get([FromBody] UsersFilter filter)
        {
            PagedResults<UserResponseDto> userslist = await _usersService.GetAllUsersAsync(filter);
            return Ok(userslist);
        }

      

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO user)
        {
            var createdUser = await _usersService.CreateUserAsync(user);

            return CreatedAtAction(
                    nameof(GetUserByID),
                    new { UserId = createdUser.Id },
                    createdUser
            );
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO UserLoginRequest)
        {
          
            var user = await _usersService.LoginAsync(UserLoginRequest.UserId);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetUserByID([FromRoute] Guid Id)
        {
            var user = await _usersService.GetUserByIdAsync(Id);
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
            Claim? UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (UserIdClaim == null)
            {
                return Unauthorized();
            }

            await _usersService.DeleteUserAsync(Guid.Parse(UserIdClaim.Value));
            return NoContent();
        }


        [HttpPut]
        [Route("Id")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] CreateUserDTO updatedUser)
        {
            Claim? UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (UserIdClaim == null)
            {
                return Unauthorized();
            }

            var user = await _usersService.UpdateUserAsync(Guid.Parse(UserIdClaim.Value), updatedUser);
            if (user == false)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
