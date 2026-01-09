using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using System.Linq.Expressions;

namespace SocialMedia.Data.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<User> _Users;
       
        public UsersRepository(SocialContext context) {
            _context = context;
            _Users = _context.Users;

        }

        public async Task<User?> GetUserByIdAsync(Guid userId) 
        {

            return await _Users.FindAsync(userId);
        }
        public async Task AddUserAsync(User user) 
        {
            await _Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResults<UserResponseDto>> GetAllUsersAsync(string? name ,int pagesize, int page, SortOrder order) 
        {

            IQueryable<User> QueryUsers;
            List<UserResponseDto> QueryResult;
            int TotalUsers;


            if (name != null)
            {
                QueryUsers = _Users
                              .Where(u => u.Username.StartsWith(name));
            }

            else {

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
                 .Select(u => u.ToDTO())
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .ToListAsync();

            }

            else
            {

                QueryResult = await QueryUsers
                 .OrderBy(u => u.Username)
                 .Select(u => u.ToDTO())
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .ToListAsync();
            }

            return new PagedResults<UserResponseDto>
            {
                Results = QueryResult,
                TotalCount = TotalUsers
            };



        }

      


        public async Task<bool> UpdateUserAsync(User user) 
        { 
            _Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
