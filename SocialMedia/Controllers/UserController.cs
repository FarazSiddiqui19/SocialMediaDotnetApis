using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.models.DTO.Users;
using SocialMedia.Services.Interfaces;


namespace SocialMedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUsersServices _usersService;
        public UserController(IUsersServices UserServices)
        {
            _usersService = UserServices;
        }



        [HttpGet]

        public async Task<IActionResult> Get() {
            var userlist = await _usersService.GetAllUsersAsync();
            return Ok(userlist);
        }

        [HttpGet("/id")]
        public async Task<IActionResult> GetUserByID(Guid UserId) {
            var user = await _usersService.GetUserByIdAsync(UserId);
            if (user == null) {
                return NotFound();
            }   
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUsersDTO user) {
            await _usersService.CreateUserAsync(user);
            return Created();
        }

        [HttpPost("/id")]
        public async Task<IActionResult> DeleteUser(Guid UserId) {
            await _usersService.DeleteUserAsync(UserId);
            return Ok();
        }






    }
}
