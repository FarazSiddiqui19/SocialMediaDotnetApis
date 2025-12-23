using SocialMedia.models.DTO.Posts;
using System;

namespace SocialMedia.models.DTO.Users
{
    public class VeiwUsersDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        
    }
}
