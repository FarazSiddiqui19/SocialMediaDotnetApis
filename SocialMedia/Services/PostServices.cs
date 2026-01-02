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
        private readonly IUsersServices _usersServices;
        private readonly IPostQueryBuilder _postQueryBuilder;


        public PostServices(IPostRepository postRepository, 
                            IUsersServices usersServices,
                            IPostQueryBuilder postQueryBuilder
            )
        {
            _postRepository = postRepository;
            _usersServices = usersServices;
            _postQueryBuilder = postQueryBuilder;
        }


        public async Task<VeiwPostsDTO> CreatePostAsync(AddPostsDTO dto)
        {
           VeiwUsersDTO? users = await _usersServices.GetUserByIdAsync(dto.UserId);


            if (users == null)
            { 
                throw new Exception("User not found");
            }

            Posts? post = dto.ToPost();
            await _postRepository.AddPostAsync(post);
         

            return post.Toveiw();

        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            Posts? posts = await _postRepository.GetPostByIdAsync(id);

            if (posts == null)
            {
                return false;
            }

            return await _postRepository.DeletePostAsync(posts);
        }

        public async Task<PagedResults<VeiwPostsDTO>> GetAllPostsAsync(PostsFilterDTO filters,
                                                                        Guid? UserId)
        {
            List<Posts>? postlist;
            postlist= await _postRepository.GetAllPostsFiltered(filters);

            if (postlist == null) { 
                
            
            }

            int totalCount = postlist.Count;
          

            List<VeiwPostsDTO> veiwPostsDTOs = postlist.Select(p =>
            {
                return p.Toveiw();
            }).ToList();

            return new PagedResults<VeiwPostsDTO>
            {
                Items = veiwPostsDTOs,
                TotalCount = totalCount,
            };

            

        }





        public async Task<VeiwPostsDTO?> GetPostByIdAsync(Guid id)
        {
            Posts? post = await _postRepository.GetPostByIdAsync(id);

            List<PostReaction>? reactions = post.Reactions;

          
            if (post == null)
            {
                return null;
            }
            return post.Toveiw();
        }

        public async Task<PagedResults<VeiwPostsDTO>> GetPostsByUserIdAsync(Guid? UserId,PostsFilterDTO filter)
        {

            List<Posts>? post = await _postRepository.GetAllPostsFiltered(filter);

            if (post == null) { 
            }

            int totalCount = post.Count;


            List<VeiwPostsDTO> veiwPostsDTOs = post.Select(p =>
            {
    
                return p.Toveiw();
            }).ToList();


            return new PagedResults<VeiwPostsDTO>
            {
                Items = veiwPostsDTOs,
                TotalCount = totalCount,
            };




        }

        public async Task<bool> UpdatePostAsync(Guid id, AddPostsDTO dto)
        {
            Posts? existingPost = await _postRepository.GetPostByIdAsync(id);
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
