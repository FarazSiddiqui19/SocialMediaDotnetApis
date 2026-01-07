using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
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

       
        public async Task<UserResponseDto> CreateUserAsync(CreateUserDTO dto)
        {
            User user = dto.ToEntity();

            await _userRepository.AddUserAsync(user);
            return user.ToDTO();

        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            
            User? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUserAsync(user);
            return true;
        }

        public async Task<PagedResults<UserResponseDto>> GetAllUsersAsync(UsersFilter filter)
        {
            string? Username = filter.Username;
            int page = filter.page;
            int pageSize = filter.pageSize;
            SortOrder orderby = filter.orderby;
          PagedResults<UserResponseDto> UsersList = await _userRepository
                                                    .GetAllUsersAsync(Username, page, pageSize, orderby);

            return UsersList;

        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            User? user  = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return user.ToDTO();
        }

        public async Task<bool> UpdateUserAsync(Guid id, CreateUserDTO dto)
        {
            User? existingUser = await _userRepository.GetUserByIdAsync(id);
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
