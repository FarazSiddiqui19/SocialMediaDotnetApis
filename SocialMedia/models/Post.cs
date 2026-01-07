using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SocialMedia.models
{
    public class Post
    {
        

        [Key] public Guid Id { get; set; }

        public Guid UserId { get; set; }

       public required string Title { get; set; }

        public PostContent Content { get; set; }=default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PostReaction>? Reactions { get; set; }

        public virtual User? User { get; set; }



        }
}
