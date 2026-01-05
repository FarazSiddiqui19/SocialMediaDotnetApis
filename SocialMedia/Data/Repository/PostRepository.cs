using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;

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
           Posts? post = await _Posts
                                .Include(p => p.Reactions).
                                Where(a=>a.PostId == postId).FirstOrDefaultAsync();
            return post;
        }

        public async Task<List<Posts>?> GetAllPosts(PostsFilterDTO filter)
        {
            IQueryable<Posts> query = _Posts
                                        .Include(p => p.Reactions);
            SortByParam sortby = filter.sortby;
            SortOrder orderby = filter.orderby;
            int page = filter.page;
            int pagesize = filter.pagesize;


           

            switch(sortby, orderby)
            {
                case (SortByParam.CreatedAt, SortOrder.Ascending):
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
                case (SortByParam.CreatedAt, SortOrder.Descending):
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                case (SortByParam.Title, SortOrder.Ascending):
                    query = query.OrderBy(p => p.Title);
                    break;
                case (SortByParam.Title, SortOrder.Descending):
                    query = query.OrderByDescending(p => p.Title);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

          
            return await query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }


        public async Task<List<Posts>?> GetAllPostsFiltered(PostsFilterDTO filter)
        {
            IQueryable<Posts> query = _Posts
                                        .Include(p => p.Reactions);
            Guid? UserId = filter.UserId;
            DateTime? fromDate = filter.FromDate;
            DateTime? toDate = filter.ToDate;
            string? title = filter.Title;
            SortByParam sortby = filter.sortby;
            SortOrder orderby = filter.orderby;
            int page = filter.page;
            int pagesize = filter.pagesize;


            if (UserId.HasValue)
            {
                query = query.Where(p => p.UserId == UserId.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= toDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.Title.Contains(title));
            }




            switch (sortby,orderby) {
                case (SortByParam.CreatedAt,SortOrder.Ascending):
                    query = query.OrderBy(p=>p.CreatedAt); 
                    break;

                case (SortByParam.CreatedAt,SortOrder.Descending):
                    query = query.OrderBy(p => p.CreatedAt);
                    break;  
                    
                case (SortByParam.Title,SortOrder.Ascending):
                    query = query.OrderBy(p => p.Title);
                    break;

                case (SortByParam.Title,SortOrder.Descending):
                    query = query.OrderByDescending(p => p.Title);
                    break;

                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

           

            return await query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();
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
