using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;

namespace SocialMedia.Data.Repository
{
    public class PostReactionRepository
    {
        private readonly SocialContext _context;

        public PostReactionRepository(SocialContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<PostReaction>> GetPostReactionAsync() { 
            return _context.PostReactions.AsQueryable();
        }
        public async Task<PostReaction> GetReactionByID(Guid reactionId) {
            
            if(reactionId == Guid.Empty)
            {
                throw new ArgumentException("Reaction ID cannot be empty.", nameof(reactionId));
            }

            var postReaction =  await _context.PostReactions
                .FirstOrDefaultAsync(r => r.Id == reactionId);

            if(postReaction == null)    
                {
                throw new KeyNotFoundException($"No PostReaction found with ID: {reactionId}");
            }

            return postReaction;
        }

        public async Task AddAsync(PostReaction reaction)
        {
            _context.PostReactions.Add(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostReaction reaction)
        {
            _context.PostReactions.Update(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PostReaction reaction)
        {
            _context.PostReactions.Remove(reaction);
            await _context.SaveChangesAsync();
        }
    }
}

