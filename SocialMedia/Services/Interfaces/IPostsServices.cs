using SocialMedia.models.DTO.Posts;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostsServices
    {
        Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto);

        Task<List<VeiwPostsDTO>> GetAllPostsAsync();
        Task<List<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid userId);


        Task<VeiwPostsDTO?> GetPostByIdAsync(Guid id);

        Task<bool> DeletePostAsync(Guid id);
    }
}
