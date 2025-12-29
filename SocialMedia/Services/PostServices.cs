using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        private readonly IPostReactionRepository _reactionRepository;
        private readonly IReactionSummaryService _reactionSummaryService;


        public PostServices(IPostRepository postRepository, 
                            IUserRepository userRepository, 
                            IPostReactionRepository reactionRepository,
                            IReactionSummaryService reactionSummaryService
            )
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _reactionRepository = reactionRepository;
            _reactionSummaryService = reactionSummaryService;
        }


        public async Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto)
        {
            var userExists = await _userRepository.GetUserByIdAsync(dto.UserId);
            var post = dto.ToPost();
            await _postRepository.AddPostAsync(post);
            var reactionSummary = await _reactionSummaryService.PostAsync(post.PostId, null);

           

            return post.Toveiw(reactionSummary);

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

        public async Task<PagedResults<VeiwPostsDTO>> GetAllPostsAsync(
                                                                    string? title,
                                                                    int page,
                                                                    int pageSize,
                                                                    SortingOrder order)
        {
          
            var postsQuery = _postRepository.PostQuery();

            
            if (!string.IsNullOrWhiteSpace(title))
            {
                postsQuery = postsQuery
                    .Where(p => p.Title.ToLower().Contains(title.ToLower()));
            }

           
            postsQuery = order == SortingOrder.Asc
                ? postsQuery.OrderBy(p => p.CreatedAt)
                : postsQuery.OrderByDescending(p => p.CreatedAt);

            
            var totalCount = await postsQuery.CountAsync();

           
            var posts =await  postsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var postIds = posts.Select(p => p.PostId).ToList();


            var reaction = await _reactionSummaryService.AllPostsAsync(postIds, null);


            var result = posts.Select(p =>
            {
   
                reaction.TryGetValue(p.PostId, out var summary);
                return p.Toveiw(
                    summary
                );
            }).ToList();

           
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

            var reactionSummary = await _reactionSummaryService.PostAsync(id,null);


            if (post == null)
            {
                return null;
            }
            return post.Toveiw(
                  reactionSummary
                );
        }

        public async Task<List<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid userId)
        {

            var post = await _postRepository
                                .PostQuery()
                                .Where(p => p.UserId == userId)
                                .ToListAsync();

            var postIds = post.Select(p => p.PostId).ToList();

            var reactionCounts = await _reactionSummaryService.AllPostsAsync(postIds, userId);

            return post.Select(p =>
            {
                reactionCounts.TryGetValue(p.PostId, out var summary);

                return p.Toveiw(
                  summary
                );
            }).ToList();


        }

        public async Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = dto.Title;
            existingPost.Content = PostContentBuilder.Build(dto.Body);
            return await _postRepository.UpdatePostAsync(existingPost);
        }


    }
}
