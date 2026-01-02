using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostsServices
    {
        Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto);

        Task<PagedResults<VeiwPostsDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                          Guid? UserId);
        Task<PagedResults<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid? UserId, PostsFilterDTO filter);


        Task<VeiwPostsDTO?> GetPostByIdAsync(Guid id);

        Task<bool> DeletePostAsync(Guid id);

        Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto);


    }
}
