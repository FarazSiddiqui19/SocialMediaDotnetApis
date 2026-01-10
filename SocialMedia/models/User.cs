using System.ComponentModel.DataAnnotations;

namespace SocialMedia.models
{
    public class User
    {
        [Key] public Guid Id { get; set; }

        public required string Username { get; set; }

        
        public  required string HashedPassword { get; set; }
        public virtual List<Post>? Posts { get; set; }

    }
}
