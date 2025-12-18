using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO.Posts;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class PostServices : IPostsServices
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostServices(IPostRepository postRepository  , IUserRepository userRepository) { 
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public Posts AddToPost(AddPostsDTO dto) {
            return new Posts
            {
                UserId = dto.UserId,

            };
        }

        public VeiwPostsDTO PostToVeiwPostsDTO(Posts post) {
            return new VeiwPostsDTO
            {
               
            };
        }

        public async Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto)
        {
            var userExists = await _userRepository.GetUserByIdAsync(dto.UserId);
            var post = AddToPost(dto);
             await _postRepository.AddPostAsync(post);

            return PostToVeiwPostsDTO(post);

        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var posts =  await _postRepository.GetPostByIdAsync(id);

            if(posts == null)
            {
                return false;
            }
            
            return await _postRepository.DeletePostAsync(posts);
        }

        public async Task<List<VeiwPostsDTO>> GetAllPostsAsync()
        {
            var posts = await  _postRepository.GetAllPostsAsync();
            return posts.Select(post => PostToVeiwPostsDTO(post)).ToList();
        }

        public async Task<VeiwPostsDTO?> GetPostByIdAsync(Guid id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return null;
            }
            return PostToVeiwPostsDTO(post);
        }

        public async Task<List<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts =  await _postRepository.GetPostsByUserIdAsync(userId);
            
            return posts.Select(PostToVeiwPostsDTO).ToList();


        }
    }
}
