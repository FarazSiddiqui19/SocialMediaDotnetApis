using System.ComponentModel.DataAnnotations;

namespace SocialMedia.models
{
    public class Posts
    {
        

        [Key] public Guid PostId { get; set; }

        public Guid UserId { get; set; }
        public required Users User { get; set; }


    }
}
