using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO.Users;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class UserServices : IUsersServices
    {
        private readonly IUserRepository  _userRepository;
        public UserServices(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

       public Users  AddToUser(AddUsersDTO dto)
        {
            return new Users
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username
            };
        }

        public VeiwUsersDTO  UserToVeiwUsersDTO(Users user)
        {
            return new VeiwUsersDTO
            {
              
                Username = user.Username
            };
        }
        public async Task<VeiwUsersDTO> CreateUserAsync(AddUsersDTO dto)
        {
            var user = AddToUser(dto);
            await _userRepository.AddUserAsync(user);
            return UserToVeiwUsersDTO(user);

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

        public async Task<List<VeiwUsersDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users
                   .Select(user => UserToVeiwUsersDTO(user))
                   .ToList();

        }

        public async Task<VeiwUsersDTO?> GetUserByIdAsync(Guid id)
        {
            var user  = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return UserToVeiwUsersDTO(user);
        }
    }
}
