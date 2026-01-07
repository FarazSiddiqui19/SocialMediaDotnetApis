using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.models.DTO.Users;
using SocialMedia.models.DTO;
using SocialMedia.Services.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


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


        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid Id)
        {
            await _usersService.DeleteUserAsync(Id);
            return NoContent();
        }


        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdateUserPut([FromRoute] Guid Id, [FromBody] CreateUserDTO updatedUser)
        {
            var user = await _usersService.UpdateUserAsync(Id, updatedUser);
            if (user == false)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
