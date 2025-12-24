using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.models.DTO.Users;
using SocialMedia.Services.Interfaces;


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



        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] string? Username)
        {
            var userlist = await _usersService.GetAllUsersAsync(Username);
            return Ok(userlist);
        }

        [HttpGet("{UserId:guid}")]
        public async Task<IActionResult> GetUserByID([FromRoute] Guid UserId)
        {
            var user = await _usersService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AddUsersDTO user)
        {
            var createdUser = await _usersService.CreateUserAsync(user);

            return CreatedAtAction(
                    nameof(GetUserByID),
                    new { UserId = createdUser.Id },
                    createdUser
            );
        }

        [HttpDelete("{UserId:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid UserId)
        {
            await _usersService.DeleteUserAsync(UserId);
            return NoContent();
        }


        [HttpPatch("{UserId:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid UserId)
        {

            return NoContent();
        }

        [HttpPut("{UserId:guid}")]
        public async Task<IActionResult> UpdateUserPut([FromRoute] Guid UserId, [FromBody] AddUsersDTO updatedUser)
        {
            var user = await _usersService.UpdateUserAsync(UserId, updatedUser);
            if (user == false)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
