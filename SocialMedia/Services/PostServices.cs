using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.models.DTO.Posts;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class PostServices : IPostsServices
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostServices(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }


        public async Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto)
        {
            var userExists = await _userRepository.GetUserByIdAsync(dto.UserId);
            var post = dto.ToPost();
            await _postRepository.AddPostAsync(post);

            return post.Toveiw();

        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var posts = await _postRepository.GetPostByIdAsync(id);

            if (posts == null)
            {
                return false;
            }

            return await _postRepository.DeletePostAsync(posts);
        }

        public async Task<List<VeiwPostsDTO>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return posts.Select(post => post.Toveiw()).ToList();
        }

        public async Task<VeiwPostsDTO?> GetPostByIdAsync(Guid id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return null;
            }
            return post.Toveiw();
        }

        public async Task<List<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);

            return posts.Select(posts => posts.Toveiw()).ToList();


        }

        public async Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = dto.Title;
            return await _postRepository.UpdatePostAsync(existingPost);
        }
    }
}
