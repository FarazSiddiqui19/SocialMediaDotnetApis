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
        private readonly IReactionSummaryService _reactionSummaryService;
        private readonly IPostQueryBuilder _postQueryBuilder;


        public PostServices(IPostRepository postRepository, 
                            IUserRepository userRepository, 
                            IReactionSummaryService reactionSummaryService,
                            IPostQueryBuilder postQueryBuilder
            )
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _reactionSummaryService = reactionSummaryService;
            _postQueryBuilder = postQueryBuilder;
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

        public async Task<PagedResults<VeiwPostsDTO>> GetAllPostsAsync(PostQueryParams queryParams,Guid? UserId)
        {
          
            IQueryable<Posts> query = _postQueryBuilder.Build(queryParams);

            int totalCount = await query.CountAsync();

            List<Posts> posts = await query
                                                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                                                .Take(queryParams.PageSize)
                                                .ToListAsync();

            List<Guid> postIds = posts.Select(p => p.PostId).ToList();

            Dictionary<Guid, ReactionSummaryDTO> reactionSummary = await _reactionSummaryService.AllPostsAsync(postIds, UserId);


            List<VeiwPostsDTO> veiwPostsDTOs = posts.Select(p =>
            {
                reactionSummary.TryGetValue(p.PostId, out var summary);
                return p.Toveiw(
                  summary
                );
            }).ToList();

            return new PagedResults<VeiwPostsDTO>
            {
                Items = veiwPostsDTOs,
                TotalCount = totalCount,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
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

        public async Task<PagedResults<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid userId)
        {

            var post = await _postRepository
                                .PostQuery()
                                .Where(p => p.UserId == userId)
                                .ToListAsync();

            int totalCount = post.Count;
            PostQueryParams queryParams = new PostQueryParams
            {
                PostsByUser = userId,
                Page = 1,
                PageSize = post.Count
            };
            IQueryable<Posts> query = _postQueryBuilder.Build(queryParams);

            List<Posts> posts = await query
                                        .Skip((queryParams.Page - 1) * queryParams.PageSize)
                                        .Take(queryParams.PageSize)
                                        .ToListAsync();
                              

            var postIds = posts.Select(p => p.PostId).ToList();

            var reactionSummary = await _reactionSummaryService.AllPostsAsync(postIds, userId);

            List<VeiwPostsDTO> veiwPostsDTOs = posts.Select(p =>
            {
                reactionSummary.TryGetValue(p.PostId, out var summary);
                return p.Toveiw(
                  summary
                );
            }).ToList();

            return new PagedResults<VeiwPostsDTO>
            {
                Items = veiwPostsDTOs,
                TotalCount = totalCount,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
            };


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
