using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.mappers;
using SocialMedia.Models;

namespace SocialMedia.Mappers
{
    public static class FriendRequestMappper
    {
        public static FriendRequest ToEntity(this RequestDTO friendRequest)
        {

            return new FriendRequest {
                 SenderId = friendRequest.SenderId,
               
                 status = Status.Pending
            
            };
        }

        public static RequestDTO ToDTO(this FriendRequest friendRequest)
        {
            return new RequestDTO
            {
                SenderId = friendRequest.SenderId,
                Sender = friendRequest.Sender.ToDTO(),
            };
        }
    }
}
