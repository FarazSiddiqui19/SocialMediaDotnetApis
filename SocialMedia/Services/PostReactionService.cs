using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO.PostReaction;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IPostReactionRepository _reactionRepo;
        private readonly IPostsServices _postServices;

        public PostReactionService(
            IPostReactionRepository reactionRepo,
            IPostsServices postServices)
        {
            _reactionRepo = reactionRepo;
            _postServices = postServices;
        }

        public async Task ToggleReactionAsync(ReactToPostDTO dto)
        {
          
            var post = await _postServices.GetPostByIdAsync(dto.PostId);
            
            if (post == null)
                throw new ArgumentException("Post not found");

            var reactionquery = _reactionRepo.GetPostReactionAsync();

            var existingReaction = reactionquery
                .FirstOrDefault(r => r.PostId == dto.PostId && r.UserId == dto.UserId);

            if (existingReaction != null)
                {
                if (existingReaction.Type == dto.Type)
                {
                    await _reactionRepo.DeleteAsync(existingReaction);
                }
                else
                {
                    existingReaction.Type = dto.Type;
                    await _reactionRepo.UpdateAsync(existingReaction);
                }
            }
            else
            {
                var newReaction = new PostReaction
                {
                    Id = Guid.NewGuid(),
                    PostId = dto.PostId,
                    UserId = dto.UserId,
                    Type = dto.Type,
                    CreatedAt = DateTime.UtcNow
                };
                await _reactionRepo.AddAsync(newReaction);
            }

        }

     
    }

}
