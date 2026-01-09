using SocialMedia.models;
using System.Text.Json.Serialization;

namespace SocialMedia.DTO.PostReaction
{
    public class ReactToPostDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

      
        public ReactionType Type { get; set; }
    }
}
