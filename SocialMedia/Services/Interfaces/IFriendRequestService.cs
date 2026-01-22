using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.Models;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Services.Interfaces
{
    public interface IFriendRequestService
    {
        Task<PagedResults<RequestDTO>> GetFriendRequestsAsync(Guid userId, RequestFilterDTO filter);
        Task AddFriendRequestAsync(Guid senderId, Guid receiverId);
        Task RespondToFriendRequestAsync(Guid SenderId, Guid recipientId, Status newStatus);
        Task DeleteFriendRequestAsync(Guid SenderId, Guid recipientId);



    }
}
