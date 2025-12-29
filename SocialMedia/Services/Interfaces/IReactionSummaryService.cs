using SocialMedia.models.DTO;

namespace SocialMedia.Services.Interfaces
{
    public interface IReactionSummaryService
    {
        Task<Dictionary<Guid, ReactionSummaryDTO>>
        AllPostsAsync(List<Guid> postIds, Guid? userId);

        Task<ReactionSummaryDTO>
            PostAsync(Guid postId, Guid? userId);
    }
}
