using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocialMedia.Services
{
    public class PostServices : IPostsServices
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostReactionRepository _postReactionRepository;




        public PostServices(IPostRepository postRepository, IPostReactionRepository postReactionRepository)
        {
            _postRepository = postRepository;
            _postReactionRepository = postReactionRepository;

        }


        public async Task<PostResponseDTO> CreatePostAsync(CreatePostDTO dto,Guid UserId)
        {


            Post? post = dto.ToEntity(UserId);
            await _postRepository.AddPostAsync(post);


            return post.ToDTO(UserId);

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

        public async Task<PagedResults<PostResponseDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                        Guid? LoggedInUser)
        {

            PagedResults<PostResponseDTO> postslist = await _postRepository.GetAllPosts(filters,LoggedInUser);

            if (postslist.TotalCount == 0)
            {
                return new PagedResults<PostResponseDTO>
                {
                    Results = new List<PostResponseDTO>(),
                    TotalCount = 0,
                };

            }

            return postslist;

        }





        public async Task<PostResponseDTO?> GetPostByIdAsync(Guid id)
        {
            Post? post = await _postRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return null;
            }

            List<PostReaction>? reactions = post.Reactions;



            return post.ToDTO(Guid.Empty);
        }



        public async Task<bool> UpdatePostAsync(Guid id, CreatePostDTO UpdatedPost)
        {
            Post? existingPost = await _postRepository.GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = UpdatedPost.Title;
            existingPost.Content = UpdatedPost.Content;
            return await _postRepository.UpdatePostAsync(existingPost);
        }

        public async Task<bool> PostReaction(ReactToPostDTO Reaction,Guid UserId)
        {
            PostReaction? existingReaction = await _postReactionRepository.GetUserReactionToPostAsync(Reaction.PostId, UserId);
           
            if (existingReaction == null)
            {
                PostReaction newReaction = Reaction.ToEntity(UserId);
                await _postReactionRepository.AddAsync(newReaction);

            }
            else
            {
                if (existingReaction.Type == Reaction.Type)
                {

                    await _postReactionRepository.DeleteAsync(existingReaction);

                }
                else
                {

                    existingReaction.Type = Reaction.Type;
                    await _postReactionRepository.UpdateAsync(existingReaction);
                }

            }

            return true;


        }


        public async Task<string> VerifyUser(ClaimsPrincipal User)
        {
            if (User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
            {
                return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
             
            }


            return string.Empty;

        }




    }
}
