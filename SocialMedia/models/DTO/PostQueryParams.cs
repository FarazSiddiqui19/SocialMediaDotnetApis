using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SocialMedia.models.DTO
{
    public class PostQueryParams
    {
        public Guid? UserId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? Search { get; set; }

        public SortOrder orderby { get; set; } = SortOrder.Ascending;

        public SortByParam SortBy { get; set; } = SortByParam.CreatedAt;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public enum SortByParam
    {
        CreatedAt,
        Title
    }   

}
