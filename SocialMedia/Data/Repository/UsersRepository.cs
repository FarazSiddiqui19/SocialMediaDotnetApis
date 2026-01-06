using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using System.Linq.Expressions;

namespace SocialMedia.Data.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<User> _Users;
        private IQueryable<User> _QueryUsers;
        public UsersRepository(SocialContext context) {
            _context = context;
            _Users = _context.Users;
            _QueryUsers = context.Users.AsQueryable();

        }

        public IQueryable<User> UserQuery() {
            return _QueryUsers;
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

        public async Task<List<User>?> GetAllUsersAsync(int pagesize, int page, SortOrder order) 
        {
            if(order == SortOrder.Descending)
            {
                return await _Users
                .OrderByDescending(u => u.Username)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();
            }

            return await _Users
                .OrderBy(u => u.Username)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }

        public async Task<List<User>?> GetUserByNameAsync(string name,int pagesize, int page,SortOrder order) 
        {

           

            if (order == SortOrder.Descending)
            {
                return await _Users
                 .Where(u => u.Username.ToLower().Contains(name.ToLower()))
                 .OrderByDescending(u => u.Username)
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .ToListAsync();
            }

          

            return await _Users
                .Where(u => u.Username.ToLower().Contains(name.ToLower()))
                .OrderBy(u => u.Username)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

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
