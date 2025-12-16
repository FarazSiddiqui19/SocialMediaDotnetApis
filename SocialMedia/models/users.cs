using System.ComponentModel.DataAnnotations;

namespace SocialMedia.models
{
    public class Users
    {
        [Key] public Guid UserId { get; set; }

        public required string Username { get; set; }

        public ICollection<Posts> Posts { get; set; } = new List<Posts>();
    }
}
