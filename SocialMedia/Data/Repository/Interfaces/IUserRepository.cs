using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
       

        Task<User?> GetUserByIdAsync(Guid userId);

        Task<PagedResults<UserResponseDto>> GetAllUsersAsync(string? name, int pagesize, int page, SortOrder order);
        Task AddUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(User user);

    }
}
