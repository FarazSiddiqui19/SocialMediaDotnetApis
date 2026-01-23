using SocialMedia.models;

namespace SocialMedia.Data.Repository.Interfaces;
public interface IPostReactionRepository : IRepository<PostReaction>
{
   

    Task<PostReaction?> GetUserReactionToPostAsync(Guid postId, Guid userId);

}

