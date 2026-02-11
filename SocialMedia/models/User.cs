using SocialMedia.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.models
{
    public class User
    {
        [Key] public Guid Id { get; set; }

        public required string Username { get; set; }

        
        public  required byte[] HashedPassword { get; set; }

        public  required string Email { get; set; }

      
        public virtual List<Post>? Posts { get; set; }

    
        public virtual List<FriendRequest>? Requests { get; set; }
   
       

    }
}
