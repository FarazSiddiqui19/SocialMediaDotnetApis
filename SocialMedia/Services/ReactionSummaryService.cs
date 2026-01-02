using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.Services.Interfaces;
using System.Collections.Generic;

namespace SocialMedia.Services
{
    public class ReactionSummaryService : IReactionSummaryService
    {
        private readonly IPostReactionRepository _reactionRepository;

        public ReactionSummaryService(IPostReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<Dictionary<Guid, ReactionSummaryDTO>> AllPostsAsync(
           List<Guid> postIds,
            Guid? userId)
        {
            if (postIds.Count == 0)
                return new Dictionary<Guid, ReactionSummaryDTO>();

            var baseQuery = _reactionRepository
                .GetPostReactionAsync()
                .Where(r => postIds.Contains(r.PostId));

           
            var summaries = await baseQuery
                .GroupBy(r => r.PostId)
                .Select(g => new ReactionSummaryDTO
                {
                    PostId = g.Key,
                    Upvotes = g.Count(r => r.Type == ReactionType.Upvote),
                    Downvotes = g.Count(r => r.Type == ReactionType.Downvote),
                 
                })
                .ToDictionaryAsync(x => x.PostId);

           
            if (userId.HasValue)
            {
                var userReactions = await baseQuery
                    .Where(r => r.UserId == userId.Value)
                    .Select(r => new { r.PostId, r.Type })
                    .ToListAsync();

                foreach (var ur in userReactions)
                {
                    if (summaries.TryGetValue(ur.PostId, out var summary))
                    {
                        
                    }
                }
            }

            return summaries;
        }

        public async Task<ReactionSummaryDTO> PostAsync(
            Guid postId,
            Guid? userId)
        {
            var result = await AllPostsAsync(
              new List<Guid> { postId },
                                userId
            );

            return result.TryGetValue(postId, out var summary)
                ? summary
                : new ReactionSummaryDTO
                {
                    PostId = postId,
                    Upvotes = 0,
                    Downvotes = 0,
               
                };
        }
    }
}

