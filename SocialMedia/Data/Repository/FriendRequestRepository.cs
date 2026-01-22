using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.FriendRequest;
using SocialMedia.DTO.Users;
using SocialMedia.Mappers;
using SocialMedia.Models;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<FriendRequest> _friendRequest;

        public FriendRequestRepository(SocialContext context)
        {
            _context = context;
            _friendRequest = _context.FriendRequest;
        }

        public async Task AddAsyn(RequestDTO request)
        {
            FriendRequest Request = request.ToEntity();
            _friendRequest.Add(Request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FriendRequest request)
        {
            _friendRequest.Remove(request);
            await _context.SaveChangesAsync();
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

        public async Task<FriendRequest> GetByIdAsync(Guid SenderId , Guid RecieverId)
        {
            return await _friendRequest.FirstOrDefaultAsync(r => (r.SenderId == SenderId && r.RecieverId == RecieverId) || 
                                                                 (r.SenderId == RecieverId && r.RecieverId == SenderId)
                                                           );
        }

        public async Task<int> GetIdBySenderReciever(Guid SenderId, Guid RecieverId)
        {
            var query = await _friendRequest.FindAsync(SenderId, RecieverId);

            return query.id;
        }

        public async Task UpdateAsync(FriendRequest request)
        {
            _friendRequest.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task AddBulkAsync(List<Guid> Senders, Guid Receiver )
        {
            List<FriendRequest> requests = new List<FriendRequest>();
            for (int i = 0; i < Senders.Count; i++)
            {
                FriendRequest request = new FriendRequest
                {
                    SenderId = Senders[i],
                    RecieverId = Receiver,
                    status = Status.Pending
                };
                requests.Add(request);
            }
            await _friendRequest.AddRangeAsync(requests);
            await _context.SaveChangesAsync();
        }
    }
}
