namespace SocialMedia.models.DTO
{
    public class ReactionSummaryDTO
    {
        public Guid PostId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
      
    }
}
