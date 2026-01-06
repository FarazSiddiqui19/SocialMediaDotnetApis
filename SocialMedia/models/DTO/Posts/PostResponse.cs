using System.Text.Json;

namespace SocialMedia.models.DTO.Posts
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public required string Title { get; set; }

        public string Content { get; set; }

        public int WordCount { get; set; }

        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public ReactionType? UserReaction { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}

