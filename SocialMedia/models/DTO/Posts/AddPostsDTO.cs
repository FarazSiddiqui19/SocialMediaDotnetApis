using System.Text.Json;

namespace SocialMedia.models.DTO.Posts
{
    public class AddPostsDTO
    {
        public Guid UserId { get; set; }
        public required string Title { get; set; }

        public PostContent Content {  get; set; }
    }
}
