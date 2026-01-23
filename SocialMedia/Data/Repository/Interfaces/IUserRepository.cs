using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
       
        Task<PagedResults<UserResponseDto>> GetAllUsersAsync(string? name, int pagesize, int page, SortOrder order);
  
        Task<Guid?> GetUserByIdEmailAsync(string Email);

        Task<User?> GetUserByEmailAsync(string Email);

    }
}
