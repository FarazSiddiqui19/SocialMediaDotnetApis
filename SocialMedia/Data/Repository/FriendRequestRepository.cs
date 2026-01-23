using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.DTO.Users;
using SocialMedia.Models;

namespace SocialMedia.Data.Repository
{
    public class FriendRequestRepository :Repository<FriendRequest>, IFriendRequestRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<FriendRequest> _friendRequest;

        public FriendRequestRepository(SocialContext context) : base(context)
        {
            _context = context;
            _friendRequest = _context.FriendRequest;
        }

    
        public async Task<PagedResults<RequestDTO>> GetAllAsyn(RequestFilterDTO filter, Guid UserId)
        {
            IQueryable<FriendRequest> query = _friendRequest
                .Where(r => r.RecieverId == UserId);

            if (filter.SenderId.HasValue)
            {
                query = query.Where(r => r.SenderId == filter.SenderId.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.status == filter.Status.Value);
            }

            var totalCount = await query.CountAsync();

            var requests = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(r => new RequestDTO
                {
                    SenderId = r.SenderId,
                    Sender = new UserResponseDto { Id = r.Sender.Id, Username = r.Sender.Username },
                    status = r.status
                })
                .ToListAsync();

            return new PagedResults<RequestDTO>
            {
                Results = requests,
                TotalCount = totalCount
            };
        }

      
        public async Task<FriendRequest?> GetByIdAsync(Guid SenderId,Guid RecieverId)
        {
            return await _friendRequest.FirstOrDefaultAsync(r => (r.SenderId == SenderId && r.RecieverId == RecieverId) ||
                                                                 (r.SenderId == RecieverId && r.RecieverId == SenderId));
        }





    }
}
