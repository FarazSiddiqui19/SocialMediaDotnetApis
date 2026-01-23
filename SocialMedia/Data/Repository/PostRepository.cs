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
    public class PostRepository : Repository<Post> , IPostRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<Post> _Posts;
       
        public PostRepository(SocialContext context) : base(context)
        {
            _context = context;
            _Posts = _context.Posts;

        }

        public override async Task<Post?> GetByIdAsync(Guid id)
        {

            Post? post = await _Posts
                                .Include(p => p.Reactions).
                                Where(a => a.Id == id).FirstOrDefaultAsync();

            return post;
        }


        public async Task<PagedResults<PostResponseDTO>> GetAllPosts(PostsFilterDTO filter, Guid? LoggedInUser)
        {
            IQueryable<Post> query = _Posts.Include(p => p.Reactions);
            //IQueryable<Post> query = _Posts.AsQueryable();


            if (filter.UserId.HasValue)
            {
                query = query.Where(p => p.AuthorId == filter.UserId.Value);
            }

           
            if (filter.FromDate.HasValue && filter.ToDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= filter.FromDate.Value && p.CreatedAt <= filter.ToDate.Value);
            }
            else if (filter.FromDate.HasValue)
            {
                
                query = query.Where(p => p.CreatedAt >= filter.FromDate.Value);
            }
            else if (filter.ToDate.HasValue)
            {
                
                query = query.Where(p => p.CreatedAt <= filter.ToDate.Value);
            }

           
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(p => p.Title.StartsWith(filter.Title));
            }

            int totalCount = await query.CountAsync();

         
            switch (filter.sortby, filter.orderby)
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

            
            List<PostResponseDTO> queryResult = await query
                .Skip((filter.page - 1) * filter.pagesize)
                .Take(filter.pagesize)
                .Select(p => p.ToDTO(LoggedInUser))
                .ToListAsync();

            return new PagedResults<PostResponseDTO>
            {
                Results = queryResult,
                TotalCount = totalCount
            };
        }


    }
}
