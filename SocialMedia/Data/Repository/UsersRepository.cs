using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.Models;

namespace SocialMedia.Data.Repository
{
    public class UsersRepository : Repository<User> ,IUserRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<User> _Users;
        private readonly DbSet<FriendRequest> _FriendRequests;

        public UsersRepository(SocialContext context):base(context)
        {
            _context = context;
            _Users = _context.Users;
            _FriendRequests = _context.FriendRequest;

        }

       
       
        public async Task<PagedResults<UserResponseDto>> GetAllUsersAsync(string? name, int pagesize, int page, SortOrder order)
        {

            IQueryable<User> QueryUsers;
            List<UserResponseDto> QueryResult;
            int TotalUsers;


            if (name != null)
            {
                QueryUsers = _Users
                              .Where(u => u.Username.StartsWith(name));
            }

            else
            {

                QueryUsers = _Users;
            }



            TotalUsers = await QueryUsers.CountAsync();

            if (TotalUsers == 0)
            {
                return new PagedResults<UserResponseDto>
                {
                    Results = new List<UserResponseDto>(),
                    TotalCount = 0
                };
            }


            if (order == SortOrder.Descending)
            {

                QueryResult = await QueryUsers
                 .OrderByDescending(u => u.Username)
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .Select(u => u.ToDTO())
                 .ToListAsync();

            }

            else
            {

                QueryResult = await QueryUsers
                 .OrderBy(u => u.Username)
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .Select(u => u.ToDTO())
                 .ToListAsync();
            }

            return new PagedResults<UserResponseDto>
            {
                Results = QueryResult,
                TotalCount = TotalUsers
            };



        }


        public async Task<Guid?> GetUserByIdEmailAsync(string Email)
        {
            return await _Users.Where(u => u.Email == Email)
                               .Select(u => u.Id)
                               .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string Email)
        {
           
            return await _Users.Where(u => u.Email == Email)
                               .FirstOrDefaultAsync();


        }

        public async Task<List<User>>? GetUserFriendListAsync(Guid UserId)
        {
            var user = _Users
                         .SelectMany(u=>u.Requests)
                          .Where(r=> r.RecieverId == UserId && r.status == Status.Accepted)
                           .Select(u=> u.Sender.Email)
                                    ;

            string query = user.ToQueryString();

          




            //return await _context.Users
            //    .Where(u => u.Id == UserId)
            //    .Select(u => u.Friends) 
            //    .FirstOrDefaultAsync();

            return null;
        
           

        }

        public async Task<bool> AddFriendRequest(Guid SenderId, Guid ReciverId)
        {

            _FriendRequests.Add(new FriendRequest
            {
                SenderId = SenderId,
                RecieverId = ReciverId,
                status = Status.Pending
            });

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> FriendRequestExists(Guid SenderId, Guid ReciverId)
        {
            return await _FriendRequests
                           .FirstOrDefaultAsync(fr => fr.SenderId == SenderId && fr.RecieverId == ReciverId)!=null;
        }


        public async Task<bool> UpdateFriendRequest(FriendRequest Request)
        {

            FriendRequest? existingRequest =  _FriendRequests.FirstOrDefault(fr => fr.SenderId == Request.SenderId && fr.RecieverId == Request.RecieverId);

            if (existingRequest == null)
                return false;


            existingRequest.status = Request.status;
            await _context.SaveChangesAsync();
            return true;
          

        }


        public async Task<PagedResults<FriendRequest>?> GetAllFriendRequests(Guid LoggedInUser,int pageSize , int page)
        {
            var requests = _FriendRequests
                           .Where(fr => fr.RecieverId == LoggedInUser);

            int totalRequests = await requests.CountAsync();

            var results =await  requests
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();


            return new PagedResults<FriendRequest>
            {
                Results = results,
                TotalCount = totalRequests
            };
        }
    }
}
