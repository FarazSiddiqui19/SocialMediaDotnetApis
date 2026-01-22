using SocialMedia.DTO.Posts;
using SocialMedia.models;
using SocialMedia.DTO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SocialMedia.mappers
{
    public static class PostMapper
    {

        public static Post ToEntity(this CreatePostDTO dto,Guid LoggedInUser)
        {

            return new Post
            {
                AuthorId = LoggedInUser,
                Title = dto.Title,
                Content = dto.Content 

            };
        }

        public static PostResponseDTO ToDTO(this Post post,Guid? LoggedInUser)
        {
           
            int upvotes = post.Reactions?.Count(r => r.Type == ReactionType.Upvote) ?? 0;
            int downvotes = post.Reactions?.Count(r => r.Type == ReactionType.Downvote) ?? 0;

            PostReaction? UserReaction = post.Reactions?
                                            .FirstOrDefault(r => r.UserId == LoggedInUser)??null;
                                            


            return new PostResponseDTO
            {
                UserId = post.AuthorId,
                Id = post.Id,
                Title = post.Title,
                WordCount = post.Content.meta.wordCount,
                Content = post.Content.markdown.content,
                Upvotes = upvotes,
                Downvotes = downvotes,
                UserReaction=UserReaction?.Type,
                CreatedAt = post.CreatedAt

            };
        }



    }
}
