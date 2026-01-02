namespace SocialMedia.models.DTO
{
    public class PagedResults<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }

    }
}


