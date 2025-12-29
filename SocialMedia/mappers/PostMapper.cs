using SocialMedia.models;
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
                Content = PostContentBuilder.Build(dto.Body),
                CreatedAt = DateTime.UtcNow

            };
        }

        public static VeiwPostsDTO Toveiw(this Posts post, int Upvotes, int Downvotes)
        {
            var meta = post.Content.RootElement.GetProperty("meta");
            var body = post.Content.RootElement.GetProperty("body");

            return new VeiwPostsDTO
            {
                UserId = post.UserId,
                PostId = post.PostId,
                Title = post.Title,
                WordCount = meta.GetProperty("wordCount").GetInt32(),
                Body = body,
                Upvotes = Upvotes,
                Downvotes = Downvotes,
                CreatedAt = post.CreatedAt

            };
        }



    }
}
