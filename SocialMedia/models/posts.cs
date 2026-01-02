using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SocialMedia.models
{
    public class Posts
    {
        

        [Key] public Guid PostId { get; set; }

        public Guid UserId { get; set; }

       public required string Title { get; set; }

        public JsonDocument Content { get; set; }=default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PostReaction>? Reactions { get; set; }

        public virtual Users? User { get; set; }



        }
}
