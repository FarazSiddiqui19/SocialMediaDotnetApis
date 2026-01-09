using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.mappers;
using SocialMedia.models;

namespace SocialMedia.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<Post> _Posts;
       
        public PostRepository(SocialContext context)
        {
            _context = context;
            _Posts = _context.Posts;

        }
        public async Task AddPostAsync(Post CreatePost)
        {
            bool UserExists = _Posts.Include(p => p.User)
                                    .Where(p=>p.UserId == CreatePost.UserId)
                                    .FirstOrDefault()!=null;

            if (UserExists == true) {
                _Posts.Add(CreatePost);
                await _context.SaveChangesAsync();
            }

            else { 
                throw new UnauthorizedAccessException("User does not exist.");

            }

        }

        public async Task<bool> DeletePostAsync(Post posts)
        {
            _Posts.Remove(posts);
            await _context.SaveChangesAsync();
            return true;
        }

  

        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
           Post? post = await _Posts
                                .Include(p => p.Reactions).
                                Where(a=>a.Id == postId).FirstOrDefaultAsync();

            if (post == null) {

                return null;
            }
            return post;
        }

       


        public async Task<PagedResults<PostResponseDTO>> GetAllPosts(PostsFilterDTO filter)
        {
            IQueryable<Post> query = _Posts
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
                if (!toDate.HasValue) {
                    toDate = fromDate.Value.AddMonths(1);
                    query = query.Where(p => p.CreatedAt >= fromDate.Value && p.CreatedAt <= toDate.Value);
                }

                else
                    query = query.Where(p => p.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                if (!fromDate.HasValue) {
                    fromDate = toDate.Value.AddMonths(-1);
                    query = query.Where(p => p.CreatedAt >= fromDate.Value && p.CreatedAt <= toDate.Value);
                }
                else
                    query = query.Where(p => p.CreatedAt <= toDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.Title.StartsWith(title));
            }

            int TotalCount = await query.CountAsync();  


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



            List<PostResponseDTO>? QueryResult = await query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                         .Select(p => p.ToDTO())
                        .ToListAsync();


            return new PagedResults<PostResponseDTO>
                {
                Results = QueryResult,
                TotalCount = TotalCount 
            };
        }


        public async Task<bool> UpdatePostAsync(Post posts)
        {
            _Posts.Update(posts);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> TestReaction(ReactToPostDTO Reaction)
        {
            var postwithreactions = _Posts
                                        .Where(p => p.Id == Reaction.PostId)
                                        .Include(p => p.Reactions);
            
                                     

            PostReaction? existingReaction = postwithreactions
                                        .SelectMany(p => p.Reactions)
                                        .Where(r => r.UserId == Reaction.UserId)
                                        .FirstOrDefault();


            return true;

        }
      

       
    }
}
