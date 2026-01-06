using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
       
        IQueryable<User> UserQuery();

        Task<User?> GetUserByIdAsync(Guid userId);
        Task<List<User>?> GetAllUsersAsync(int pagesize,int page,SortOrder ord);

        Task<List<User>?> GetUserByNameAsync(string name,int pagesize,int page,SortOrder ord);
        Task AddUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(User user);

    }
}
