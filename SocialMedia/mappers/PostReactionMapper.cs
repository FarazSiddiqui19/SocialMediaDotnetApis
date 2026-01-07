using SocialMedia.models;
using SocialMedia.models.DTO.PostReaction;

namespace SocialMedia.mappers
{
    public static class PostReactionMapper
    {

        public static PostReaction ToEntity(this ReactToPostDTO dto)
        { 
        
            return new PostReaction
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                Type = dto.Type,
            };
        }

        public static ReactToPostDTO ToDTO(this PostReaction reaction)
        {
            return new ReactToPostDTO
            {
                PostId = reaction.PostId,
                UserId = reaction.UserId,
                Type = reaction.Type,
            };
        }
    }
}
