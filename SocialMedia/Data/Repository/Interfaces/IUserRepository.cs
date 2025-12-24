using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsersAsync();

        Task<List<Users>> GetUsersByNameAsync(string? Username);
        Task<Users?> GetUserByIdAsync(Guid userId);
        Task AddUserAsync(Users user);

        Task<bool> UpdateUserAsync(Users user);

        Task<bool> DeleteUserAsync(Users user);

    }
}
