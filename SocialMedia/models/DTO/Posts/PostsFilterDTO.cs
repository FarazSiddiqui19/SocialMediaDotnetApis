using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SocialMedia.models.DTO.Posts
{
    public class PostsFilterDTO
    {
        public Guid? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? Title { get; set; }

        public SortByParam sortby { get; set; } = SortByParam.CreatedAt;

        public SortOrder orderby { get; set; } = SortOrder.Ascending;

        public int page { get; set; } = 1;

        public int pagesize { get; set; } = 20;
    }
}
