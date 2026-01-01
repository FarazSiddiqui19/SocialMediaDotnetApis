using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
       
        IQueryable<Users> UserQuery();

        Task<Users?> GetUserByIdAsync(Guid userId);

        Task<List<Users>?> GetUserByNameAsync(string name,int pagesize,int page);
        Task AddUserAsync(Users user);

        Task<bool> UpdateUserAsync(Users user);

        Task<bool> DeleteUserAsync(Users user);

    }
}
