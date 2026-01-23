using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;


namespace SocialMedia.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _repository;

        public FriendRequestService(IFriendRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task AddFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            if (senderId == receiverId)
            {
                throw new ArgumentException("Sender and receiver cannot be the same person.");
            }

            var request = await _repository.GetByIdAsync(senderId, receiverId);

            if(request != null)
            {
                throw new InvalidOperationException("A friend request already exists between these users.");
            }

            var requestDto = new RequestDTO
            {
                SenderId = senderId
                
            };
            var entity = new FriendRequest
            {
                SenderId = senderId,
                RecieverId = receiverId,
                status = Status.Pending
            };
            await _repository.AddAsync(entity);
        }

      

        public async Task RespondToFriendRequestAsync(Guid SenderId, Guid recipientId, Status newStatus)
        {
            var request = await _repository.GetByIdAsync(SenderId,recipientId);
            if (request == null)
            {
                throw new ArgumentException("Friend request not found.");
            }

            if (request.RecieverId != recipientId)
            {
                throw new UnauthorizedAccessException("You are not authorized to respond to this friend request.");
            }

            if (request.status != Status.Pending)
            {
                throw new InvalidOperationException("This request has already been responded to.");
            }

            if (newStatus != Status.Accepted && newStatus != Status.Rejected)
            {
                throw new ArgumentException("Invalid status update provided.");
            }

            request.status = newStatus;
            await _repository.UpdateAsync(request);
        }

        public async Task DeleteFriendRequestAsync(Guid SenderId, Guid recipientId)
        {
            var request = await _repository.GetByIdAsync(SenderId, recipientId);
            if (request == null)
            {
              
                throw new ArgumentException("Friend request not found.");
            }

            if (request.SenderId != SenderId && request.RecieverId != recipientId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this friend request.");
            }

            await _repository.DeleteAsync(request);
        }

        public async Task<PagedResults<RequestDTO>> GetFriendRequestsAsync(Guid userId, RequestFilterDTO filter)
        {
            return await _repository.GetAllAsyn(filter, userId);
        }
    }
}
