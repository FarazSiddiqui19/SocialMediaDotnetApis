using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Repository.Interfaces;

namespace SocialMedia.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SocialContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(SocialContext context ) 
        { 
            _context = context;
            _dbSet =_context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
           
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id) 
        {
            return await _dbSet.FindAsync(id);    

        }
    }
}
