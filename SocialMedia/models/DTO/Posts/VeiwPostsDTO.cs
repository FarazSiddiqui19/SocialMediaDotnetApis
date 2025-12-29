using System.Text.Json;

namespace SocialMedia.models.DTO.Posts
{
    public class VeiwPostsDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public required string Title { get; set; }

        public JsonElement Body { get; set; }

        public int WordCount { get; set; }

        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public ReactionType? UserReaction { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}

