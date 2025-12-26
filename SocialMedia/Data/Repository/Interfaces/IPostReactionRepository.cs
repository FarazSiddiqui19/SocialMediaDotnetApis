using SocialMedia.models;

namespace SocialMedia.Data.Repository.Interfaces;
public interface IPostReactionRepository
{
    Task<IQueryable<PostReaction>> GetPostAsync();

    Task<PostReaction> GetReactionByID(Guid reactionId);
    Task AddAsync(PostReaction reaction);
    Task UpdateAsync(PostReaction reaction);
    Task DeleteAsync(PostReaction reaction);
}

