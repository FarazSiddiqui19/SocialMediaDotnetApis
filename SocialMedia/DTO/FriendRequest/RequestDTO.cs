using SocialMedia.DTO.Users;
using SocialMedia.Models;

namespace SocialMedia.DTO.FriendRequest
{
    public class RequestDTO
    {
        public Guid SenderId {  get; set; }
        public UserResponseDto Sender { get; set; }

       

        public Status? status { get; set; } 
    }
}
