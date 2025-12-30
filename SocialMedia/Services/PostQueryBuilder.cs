using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.models.DTO;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class PostQueryBuilder : IPostQueryBuilder
    {
        private readonly IPostRepository _postRepository;

        public PostQueryBuilder(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IQueryable<Posts> Build(PostQueryParams p)
        {
            IQueryable<Posts> query = _postRepository.PostQuery();
            query = ApplyUserFilter(query, p);
            query = ApplySearch(query, p);
            query = ApplyDateFilter(query, p);
            query = ApplySorting(query, p);

            return query;
        }

       

        private static IQueryable<Posts> ApplyUserFilter(
            IQueryable<Posts> query, PostQueryParams p)
        {
            if (p.UserId.HasValue)
                query = query.Where(x => x.UserId == p.UserId);

            return query;
        }

        private static IQueryable<Posts> ApplySearch(
            IQueryable<Posts> query, PostQueryParams p)
        {
            if (!string.IsNullOrWhiteSpace(p.Search))
                query = query.Where(x =>
                    x.Title.Contains(p.Search));

            return query;
        }

        private static IQueryable<Posts> ApplyDateFilter(
            IQueryable<Posts> query, PostQueryParams p)
        {
            if (p.FromDate.HasValue)
                query = query.Where(x => x.CreatedAt >= p.FromDate);

            if (p.ToDate.HasValue)
                query = query.Where(x => x.CreatedAt <= p.ToDate);

            return query;
        }

        private static IQueryable<Posts> ApplySorting(
            IQueryable<Posts> query, PostQueryParams p)
        {
            return (p.SortBy, p.orderby) switch
            {
                (SortByParam.CreatedAt, SortOrder.Ascending)
                    => query.OrderBy(x => x.CreatedAt),

                (SortByParam.CreatedAt, SortOrder.Descending)
                    => query.OrderByDescending(x => x.CreatedAt),

                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }
    }

}
