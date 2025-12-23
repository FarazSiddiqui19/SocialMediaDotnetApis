namespace SocialMedia.models.DTO.Posts
{
    public class VeiwPostsDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
