using SocialMedia.Models;
using System;

namespace SocialMedia.DTO.FriendRequest
{
    public class RequestFilterDTO
    {
        public Guid? SenderId { get; set; }
        public Status? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}