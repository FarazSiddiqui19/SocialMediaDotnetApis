using SocialMedia.models;
namespace SocialMedia.Data.Repository.Interfaces
{
    public interface IPostRepository
    {
       
       
        Task<Posts?> GetPostByIdAsync(Guid postId);
        Task AddPostAsync(Posts post);
        Task<bool> UpdatePostAsync(Posts posts);
        Task<bool> DeletePostAsync(Posts post);
        IQueryable<Posts> PostQuery();
    }
}
