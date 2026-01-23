using SocialMedia.DTO;
using SocialMedia.DTO.Posts;
using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<PagedResults<PostResponseDTO>> GetAllPosts(PostsFilterDTO filter,Guid? LoggedInUser);

    }
}
