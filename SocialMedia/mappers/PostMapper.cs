using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SocialMedia.mappers
{
    public static class PostMapper
    {

        public static Post ToEntity(this CreatePostDTO dto)
        {

            return new Post
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Content = dto.Content 

            };
        }

        public static PostResponseDTO ToDTO(this Post post)
        {
           
            int upvotes = post.Reactions?.Count(r => r.Type == ReactionType.Upvote) ?? 0;
            int downvotes = post.Reactions?.Count(r => r.Type == ReactionType.Downvote) ?? 0;
           



            return new PostResponseDTO
            {
                UserId = post.UserId,
                Id = post.Id,
                Title = post.Title,
                WordCount = post.Content.meta.wordCount,
                Content = post.Content.markdown.content,
                Upvotes = upvotes,
                Downvotes = downvotes,
                CreatedAt = post.CreatedAt

            };
        }



    }
}
