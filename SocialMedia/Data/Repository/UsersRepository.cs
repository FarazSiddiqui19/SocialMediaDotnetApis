using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;

namespace SocialMedia.Data.Repository
{
    public class UsersRepository : Repository<User> ,IUserRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<User> _Users;

        public UsersRepository(SocialContext context):base(context)
        {
            _context = context;
            _Users = _context.Users;

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
    }
}
