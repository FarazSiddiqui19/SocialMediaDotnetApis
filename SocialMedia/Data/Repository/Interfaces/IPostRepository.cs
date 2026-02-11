using SocialMedia.DTO;
using SocialMedia.DTO.Posts;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<PagedResults<PostResponseDTO>> GetAllPosts(PostsFilterDTO filter, Guid? LoggedInUser);
        Task<PostReaction?> GetUserReaction(Guid PostId, Guid UserId);
        Task<bool> AddReaction(PostReaction reaction);
        Task<bool> RemoveReaction(PostReaction reaction);

        Task<bool> UpdateReaction(PostReaction reaction);
    }
}
