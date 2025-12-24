using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.models.DTO.Users;
using SocialMedia.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace SocialMedia.Services
{
    public class UserServices : IUsersServices
    {
        private readonly IUserRepository  _userRepository;
       
        public UserServices(IUserRepository userRepository) {
            _userRepository = userRepository;
          
        }

       
        public async Task<VeiwUsersDTO> CreateUserAsync(AddUsersDTO dto)
        {
            var user = dto.ToUser();
            await _userRepository.AddUserAsync(user);
            return user.Toveiw();

        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUserAsync(user);
            return true;
        }

        public async Task<List<VeiwUsersDTO>> GetAllUsersAsync(string? Username)
        {
            var users = _userRepository.UserQuery();

            if (!string.IsNullOrEmpty(Username))
            {
                users = users.Where(u => u.Username.Contains(Username));

            }

            return await users
                .Select(u => u.Toveiw())
                .ToListAsync();
        }

        public async Task<VeiwUsersDTO?> GetUserByIdAsync(Guid id)
        {
            var user  = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return user.Toveiw();
        }

        public async Task<bool> UpdateUserAsync(Guid id, AddUsersDTO dto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Username = dto.Username;
            await _userRepository.UpdateUserAsync(existingUser);
            return true;
        }
    }
}
