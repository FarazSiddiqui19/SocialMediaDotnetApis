using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Posts;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SocialMedia.mappers
{
    public static class PostMapper
    {

        public static Posts ToPost(this AddPostsDTO dto)
        {

            return new Posts
            {
                UserId = dto.UserId,
                PostId = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content ,
                CreatedAt = DateTime.UtcNow

            };
        }

        public static VeiwPostsDTO Toveiw(this Posts post)
        {
           
            var upvotes = post.Reactions?.Count(r => r.Type == ReactionType.Upvote) ?? 0;
            var downvotes = post.Reactions?.Count(r => r.Type == ReactionType.Downvote) ?? 0;
           



            return new VeiwPostsDTO
            {
                UserId = post.UserId,
                PostId = post.PostId,
                Title = post.Title,
                WordCount = post.Content.meta.wordCount,
                Body = post.Content.markdown.content,
                Upvotes = upvotes,
                Downvotes = downvotes,
                CreatedAt = post.CreatedAt

            };
        }



    }
}
