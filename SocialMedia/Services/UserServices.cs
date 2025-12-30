using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedResults<VeiwUsersDTO>> GetAllUsersAsync(string? Username, int page, int pageSize, SortingOrder ord)
        {
            var users = _userRepository.UserQuery();

            if (!string.IsNullOrEmpty(Username))
            {
                users = users.Where(u => u.Username.ToLower().Contains(Username.ToLower()));

            }

            if(ord == SortingOrder.Desc)
            {
                users = users.OrderByDescending(u => u.Username);
            }
            else
            {
                users = users.OrderBy(u => u.Username);
            }

            var totalCount = await users.CountAsync();

            var result = await users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u=>u.Toveiw())
                .ToListAsync();

            if(result.Count == 0)
            {
                return new PagedResults<VeiwUsersDTO>
                {
                    Items = new List<VeiwUsersDTO>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }



            return new PagedResults<VeiwUsersDTO>
            {
                Items = result,
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
