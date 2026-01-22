using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.DTO.Users;
using SocialMedia.Models;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IFriendRequestRepository
    {
        Task AddAsyn(RequestDTO request);

        Task UpdateAsync(FriendRequest request);

        Task DeleteAsync(FriendRequest request);

        Task<PagedResults<RequestDTO>> GetAllAsyn(RequestFilterDTO filter, Guid UserId);

        Task<FriendRequest> GetByIdAsync(Guid SenderId, Guid RecieverId);

        Task AddBulkAsync(List<Guid> Senders, Guid Receiver);
    }
}
