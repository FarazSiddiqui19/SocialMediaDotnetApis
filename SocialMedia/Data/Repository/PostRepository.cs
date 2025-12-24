using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;

namespace SocialMedia.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<Posts> _Posts;
        private IQueryable<Posts> _QueryPosts;
        public PostRepository(SocialContext context)
        {
            _context = context;
            _Posts = _context.Posts;
            _QueryPosts = context.Posts.AsQueryable();

        }
        public async Task AddPostAsync(Posts post)
        {
            _Posts.Add(post);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> DeletePostAsync(Posts posts)
        {
            _Posts.Remove(posts);
            await _context.SaveChangesAsync();
            return true;
        }

  

        public async Task<Posts?> GetPostByIdAsync(Guid postId)
        {

            return await _Posts.FindAsync(postId);
        }

        public async Task<bool> UpdatePostAsync(Posts posts)
        {
            _Posts.Update(posts);
            await _context.SaveChangesAsync();
            return true;
        }

      

        public IQueryable<Posts> PostQuery()
        {
            return _QueryPosts;
        }
    }
}
