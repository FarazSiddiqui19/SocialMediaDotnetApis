using SocialMedia.models.DTO.Posts;
using System;

namespace SocialMedia.models.DTO.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        
    }
}
