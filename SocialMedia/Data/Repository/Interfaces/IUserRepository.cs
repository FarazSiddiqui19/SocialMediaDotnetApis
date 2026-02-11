using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.models;
using SocialMedia.Models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
       
        Task<PagedResults<UserResponseDto>> GetAllUsersAsync(string? name, int pagesize, int page, SortOrder order);
  
        Task<Guid?> GetUserByIdEmailAsync(string Email);

        Task<User?> GetUserByEmailAsync(string Email);

        Task<List<User>>? GetUserFriendListAsync(Guid UserId);

        Task<bool> AddFriendRequest(Guid SenderId, Guid ReciverId);

        Task<bool> UpdateFriendRequest(FriendRequest Request);

        Task<PagedResults<FriendRequest>?> GetAllFriendRequests(Guid LoggedInUser, int pageSize, int page);

        Task<bool> FriendRequestExists(Guid SenderId, Guid ReciverId);

    }
}
