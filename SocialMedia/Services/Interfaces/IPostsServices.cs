using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostsServices
    {
        Task<PostResponse> CreatePostAsync(AddPostsDTO dto);

        Task<PagedResults<PostResponse>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                          Guid? UserId);
        Task<PostResponse?> GetPostByIdAsync(Guid id);

        Task<bool> DeletePostAsync(Guid id);

        Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto);


    }
}
