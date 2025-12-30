using SocialMedia.models;
using SocialMedia.models.DTO;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostQueryBuilder
    {
        IQueryable<Posts> Build(PostQueryParams parameters);
    }
}
