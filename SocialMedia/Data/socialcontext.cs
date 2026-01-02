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

        public DbSet<PostReaction> PostReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasOne(p => p.User).WithMany(a => a.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostReaction>(entity =>
            {
                entity.HasKey(r => r.Id);


                entity.HasIndex(r => new { r.PostId, r.UserId })
                      .IsUnique();


              entity.HasOne<Posts>()
                  .WithMany(p => p.Reactions)
                  .HasForeignKey(r => r.PostId)
                  .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne<Users>()
                  .WithMany()
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            });



            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.Content).HasColumnType("jsonb");
            });
        }

    }
}
