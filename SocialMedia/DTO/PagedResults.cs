namespace SocialMedia.DTO
{
    public class PagedResults<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }

    }
}


