using Microsoft.EntityFrameworkCore;
using SocialMedia.models;

namespace SocialMedia.Data
{
    public class SocialContext : DbContext
    {
        public SocialContext(DbContextOptions<SocialContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posts>()
                .HasOne<Users>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
