using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.DTO.Users;
using SocialMedia.Services.Interfaces;
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


        public async Task<PostResponseDTO> CreatePostAsync(CreatePostDTO dto)
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

        public async Task<PagedResults<PostResponseDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                        Guid? UserId)
        {

            PagedResults<PostResponseDTO> postslist = await _postRepository.GetAllPosts(filters);

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



            return post.ToDTO();
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

        public async Task<bool> PostReaction(ReactToPostDTO Reaction)
        {
            PostReaction? existingReaction = await _postReactionRepository.GetUserReactionToPostAsync(Reaction.PostId, Reaction.UserId);
            var testing = await _postRepository.TestReaction(Reaction);
            if (existingReaction == null)
            {
                PostReaction newReaction = Reaction.ToEntity();
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




    }
}
