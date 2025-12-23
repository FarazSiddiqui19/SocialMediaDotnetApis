using SocialMedia.models;
using SocialMedia.models.DTO.Posts;

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
                CreatedAt = DateTime.UtcNow

            };
        }

        public static VeiwPostsDTO Toveiw(this Posts post)
        {
            return new VeiwPostsDTO
            {
                UserId = post.UserId,
                PostId = post.PostId,
                Title = post.Title,
                CreatedAt = post.CreatedAt

            };
        }

    }
}
