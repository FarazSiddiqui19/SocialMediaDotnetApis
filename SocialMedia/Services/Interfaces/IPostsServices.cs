using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using System.Security.Claims;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostsServices
    {
        Task<PostResponseDTO> CreatePostAsync(CreatePostDTO dto,Guid UserId);

        Task<PagedResults<PostResponseDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                          Guid? UserId);
        Task<PostResponseDTO?> GetPostByIdAsync(Guid id);

        Task<bool> DeletePostAsync(Guid id);

        Task<bool> UpdatePostAsync(Guid id, CreatePostDTO dto);

        Task<bool> PostReaction(ReactToPostDTO Reaction,Guid UserId);

   

     


    }
}
