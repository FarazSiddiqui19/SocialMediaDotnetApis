using SocialMedia.models.DTO.Posts;
using System;

namespace SocialMedia.models.DTO.Users
{
    public class VeiwUsersDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<VeiwPostsDTO> Posts { get; set; } = new List<VeiwPostsDTO>();
    }
}
