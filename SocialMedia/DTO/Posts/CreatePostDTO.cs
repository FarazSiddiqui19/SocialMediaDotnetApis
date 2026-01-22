using SocialMedia.models;
using System.Text.Json;

namespace SocialMedia.DTO.Posts
{
    public class CreatePostDTO
    {
       
        public required string Title { get; set; }

        public PostContent Content {  get; set; }
    }
}
