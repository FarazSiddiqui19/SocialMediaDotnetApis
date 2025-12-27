using SocialMedia.models.DTO.PostReaction;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostReactionService
    {
        Task ToggleReactionAsync(ReactToPostDTO dto);
      

    }
}
