using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Data
{
    public class SocialContext : DbContext
    {
        public SocialContext(DbContextOptions<SocialContext> options) : base(options)
        {
        }
        public DbSet<models.Users> Users { get; set; }
        public DbSet<models.Posts> Posts { get; set; }
    }
}
