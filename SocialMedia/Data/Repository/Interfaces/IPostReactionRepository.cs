using SocialMedia.models;

namespace SocialMedia.Data.Repository.Interfaces;
public interface IPostReactionRepository
{
    IQueryable<PostReaction> GetPostReactionAsync();

    Task<PostReaction?> GetUserReactionToPostAsync(Guid postId, Guid userId);

    Task<PostReaction> GetReactionByID(Guid reactionId);
    Task AddAsync(PostReaction reaction);
    Task UpdateAsync(PostReaction reaction);
    Task DeleteAsync(PostReaction reaction);
}

