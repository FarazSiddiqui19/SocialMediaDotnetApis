using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.PostReaction;
using SocialMedia.DTO.Posts;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;

namespace SocialMedia.Services
{
    public class PostServices : IPostsServices
    {
        private readonly IPostRepository _postRepository;
      




        public PostServices(IPostRepository postRepository)
        {
            _postRepository = postRepository;

        }


        public async Task<PostResponseDTO> CreatePostAsync(CreatePostDTO dto,Guid UserId)
        {


            Post? post = dto.ToEntity(UserId);
            await _postRepository.AddAsync(post);


            return post.ToDTO(UserId);

        }

      

        public async Task<bool> DeletePostAsync(Guid id)
        {
            Post? post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return false;
            }

            await _postRepository.DeleteAsync(post);
            return true;
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
            Post? post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return null;
            }

            return post.ToDTO(Guid.Empty);
        }



        public async Task<bool> UpdatePostAsync(Guid id, CreatePostDTO UpdatedPost)
        {
            Post? existingPost = await _postRepository.GetByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = UpdatedPost.Title;
            existingPost.Content = UpdatedPost.Content;
            await _postRepository.UpdateAsync(existingPost);
            return true;
        }

        public async Task<bool> PostReaction(ReactToPostDTO Reaction,Guid UserId)
        {
            PostReaction? existingReaction = await _postRepository.GetUserReaction(Reaction.PostId, UserId);
           
            if (existingReaction == null)
            {
                PostReaction newReaction = Reaction.ToEntity(UserId);
                await _postRepository.AddReaction(newReaction);

            }
            else
            {
                if (existingReaction.Type == Reaction.Type)
                {

                    await _postRepository.RemoveReaction(existingReaction);

                }
                else
                {

                    existingReaction.Type = Reaction.Type;
                    await _postRepository.UpdateReaction(existingReaction);
                }

            }

            return true;


        }





    }
}
