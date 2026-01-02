using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository
    {
       
       
        Task<Posts?> GetPostByIdAsync(Guid postId);

        Task<List<Posts>?> GetAllPosts(PostsFilterDTO filter);

        Task<List<Posts>?> GetAllPostsFiltered(PostsFilterDTO filter);
        Task AddPostAsync(Posts post);
        Task<bool> UpdatePostAsync(Posts posts);
        Task<bool> DeletePostAsync(Posts post);
        IQueryable<Posts> PostQuery();
    }
}
