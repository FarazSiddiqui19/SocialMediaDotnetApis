using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SocialMedia.DTO.Users
{
    public class UsersFilter
    {
        public string? Username { get; set;  }
        public int page { get; set; } = 1;

        public int pageSize { get; set; } = 20;

        public SortOrder orderby { get; set; } = SortOrder.Ascending;

    }
}
