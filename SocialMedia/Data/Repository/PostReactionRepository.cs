using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;

namespace SocialMedia.Data.Repository
{
    public class PostReactionRepository: Repository<PostReaction>, IPostReactionRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<PostReaction> _postReactions;

        public PostReactionRepository(SocialContext context) : base(context)
        {
            _context = context;
            _postReactions = _context.PostReactions;
        }
       

        public async Task<PostReaction?> GetUserReactionToPostAsync(Guid postId, Guid userId)
        {
            return await _postReactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
        }

      
    }
}

