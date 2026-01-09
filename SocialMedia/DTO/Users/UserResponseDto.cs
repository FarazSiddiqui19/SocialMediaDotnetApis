using SocialMedia.DTO.Posts;
using System;

namespace SocialMedia.DTO.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        
    }
}
