using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository
    {
   
        Task<Post?> GetPostByIdAsync(Guid postId);
        Task<PagedResults<PostResponseDTO>> GetAllPosts(PostsFilterDTO filter);
        Task AddPostAsync(Post post);
        Task<bool> UpdatePostAsync(Post posts);
        Task<bool> DeletePostAsync(Post post);

        Task<bool> TestReaction(ReactToPostDTO Reaction);


    }
}
