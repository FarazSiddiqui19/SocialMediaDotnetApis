using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.Models;

namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IFriendRequestRepository : IRepository<FriendRequest>
    {
        Task<PagedResults<RequestDTO>> GetAllAsyn(RequestFilterDTO filter, Guid UserId);

        Task<FriendRequest?> GetByIdAsync(Guid SenderId, Guid RecieverId);


    }
}
