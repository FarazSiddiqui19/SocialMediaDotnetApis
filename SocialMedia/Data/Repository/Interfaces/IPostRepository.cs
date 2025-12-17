using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository
    {
       
        Task<List<Posts>> GetAllPostsAsync();
        Task<Posts?> GetPostByIdAsync(Guid postId);
        Task AddPostAsync(Posts post);
        Task<bool> UpdatePostAsync(Guid postId);
        Task<bool> DeletePostAsync(Guid postId);
    }
}
