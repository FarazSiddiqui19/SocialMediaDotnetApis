using SocialMedia.DTO.PostReaction;
using SocialMedia.models;

namespace SocialMedia.mappers
{
    public static class PostReactionMapper
    {

        public static PostReaction ToEntity(this ReactToPostDTO dto,Guid UserId)
        { 
        
            return new PostReaction
            {
                PostId = dto.PostId,
                UserId = UserId,
                Type = dto.Type,
            };
        }

        public static ReactToPostDTO ToDTO(this PostReaction reaction)
        {
            return new ReactToPostDTO
            {
                PostId = reaction.PostId,
                Type = reaction.Type,
            };
        }
    }
}
