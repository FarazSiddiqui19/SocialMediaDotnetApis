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

        public async Task<PagedResults<VeiwUsersDTO>> GetAllUsersAsync(string? Username, int page, int pageSize, SortOrder ord)
        {
            List<Users>? users;
            int totalCount = 0;
            if (string.IsNullOrEmpty(Username)) {
                users = await _userRepository.GetAllUsersAsync(pageSize, page, ord);

            }

            else
            {
                users = await _userRepository.GetUserByNameAsync(Username!, pageSize, page, ord);
            }


            if (users!.Count == 0 || users == null)
            {
                totalCount = 0;

            }

            else { 
                totalCount = users.Count;
            }

             
            var veiwUsers = users!.Select(u => u.Toveiw()).ToList();
                return new PagedResults<VeiwUsersDTO>
                {
                    Items = veiwUsers,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
          
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

            try
            {
                await _userRepository.UpdateUserAsync(existingUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userRepository.GetUserByIdAsync(id) == null)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            await _userRepository.UpdateUserAsync(existingUser);

            return true;
        }


    }
}
