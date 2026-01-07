using System.Text.Json.Serialization;

namespace SocialMedia.models
{
    public class PostReaction
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public ReactionType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    }


    public enum ReactionType
    {
        Upvote=0,
        Downvote=1
    }

}
