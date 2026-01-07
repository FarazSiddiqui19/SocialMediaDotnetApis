using SocialMedia.models.DTO;
using SocialMedia.models.DTO.PostReaction;
using SocialMedia.models.DTO.Posts;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostsServices
    {
        Task<PostResponseDTO> CreatePostAsync(CreatePostDTO dto);

        Task<PagedResults<PostResponseDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                          Guid? UserId);
        Task<PostResponseDTO?> GetPostByIdAsync(Guid id);

        Task<bool> DeletePostAsync(Guid id);

        Task<bool> UpdatePostAsync(Guid id, CreatePostDTO dto);

        Task<bool> PostReaction(ReactToPostDTO Reaction);


    }
}
