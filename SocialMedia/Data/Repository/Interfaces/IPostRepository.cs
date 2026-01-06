using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository
    {
   
        Task<Post?> GetPostByIdAsync(Guid postId);
        Task<PagedResults<PostResponse>> GetAllPosts(PostsFilterDTO filter);
        Task AddPostAsync(Post post);
        Task<bool> UpdatePostAsync(Post posts);
        Task<bool> DeletePostAsync(Post post);
       
    }
}
