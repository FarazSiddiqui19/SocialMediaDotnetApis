using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.models;
using SocialMedia.Data;

namespace SocialMedia.Data.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly SocialContext _context;
        private readonly DbSet<Users> _Users;
        public UsersRepository(SocialContext context) {
            _context = context;
            _Users = _context.Users;

        }

        public async Task<List<Users>> GetAllUsersAsync() { 
           return  await _Users.ToListAsync();

        }

        public async Task<Users?> GetUserByIdAsync(Guid userId) 
        {
            return await _Users.FindAsync(userId);
        }
        public async Task AddUserAsync(Users user) 
        {
            await _Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserAsync(Users user) 
        { 
            _Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Users user)
        {
            _Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
