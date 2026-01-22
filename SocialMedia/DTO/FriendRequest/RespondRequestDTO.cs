using SocialMedia.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.DTO.FriendRequest
{
    public class RespondRequestDTO
    {
        [Required]
        public Status NewStatus { get; set; }
    }
}
