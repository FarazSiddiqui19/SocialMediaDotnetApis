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

            };
        }

        public static VeiwPostsDTO Toveiw(this Posts post)
        {
            return new VeiwPostsDTO
            {

            };
        }

    }
}
