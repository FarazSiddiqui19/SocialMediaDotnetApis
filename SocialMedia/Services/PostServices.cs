using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
using SocialMedia.models.DTO.Users;
using SocialMedia.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocialMedia.Services
{
    public class PostServices : IPostsServices
    {
        private readonly IPostRepository _postRepository;
     
       


        public PostServices(IPostRepository postRepository)
        {
            _postRepository = postRepository;
          
        }


        public async Task<PostResponse> CreatePostAsync(AddPostsDTO dto)
        {
          

            Post? post = dto.ToEntity();
            await _postRepository.AddPostAsync(post);
         

            return post.ToDTO();

        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            Post? post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return false;
            }

            return await _postRepository.DeletePostAsync(post);
        }

        public async Task<PagedResults<PostResponse>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                        Guid? UserId)
        {
           
            PagedResults<PostResponse> postslist = await _postRepository.GetAllPosts(filters);

            if (postslist.TotalCount == 0) { 
                return new PagedResults<PostResponse>
                {
                    Results = new List<PostResponse>(),
                    TotalCount = 0,
                };

            }

            return postslist;

        }





        public async Task<PostResponse?> GetPostByIdAsync(Guid id)
        {
            Post? post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return null;
            }

            List<PostReaction>? reactions = post.Reactions;

          
           
            return post.ToDTO();
        }

     

        public async Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto)
        {
            Post? existingPost = await _postRepository.GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = dto.Title;
            existingPost.Content = dto.Content;
            return await _postRepository.UpdatePostAsync(existingPost);
        }


    }
}
