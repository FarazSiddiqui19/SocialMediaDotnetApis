using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
using SocialMedia.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PagedResults<VeiwPostsDTO>> GetAllPostsAsync(string? Title, int page, int pageSize, SortingOrder order)
        {
            var posts = _postRepository.PostQuery();

            if (!string.IsNullOrWhiteSpace(Title))
            {
                posts = posts.Where(p => p.Title.ToLower().Contains(Title.ToLower()));
            }

            if(order == SortingOrder.Asc)
            {
                posts = posts.OrderBy(p => p.CreatedAt);
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedAt);
            }

            var totalCount = await posts.CountAsync();

            var result = await posts
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Select(p => p.Toveiw())
                          .ToListAsync();

            return new PagedResults<VeiwPostsDTO>
            {
                Items = result,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
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

            var post = _postRepository.PostQuery()
                                         .Where(p => p.UserId == userId);

            return post.Select(posts => posts.Toveiw()).ToList();


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
