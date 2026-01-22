using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.models;

namespace SocialMedia.Data.Config
{
    public class PostReactionConfig : IEntityTypeConfiguration<PostReaction>
    {


        public void Configure(EntityTypeBuilder<PostReaction> builder)
        {
            builder.HasKey(r => new { r.PostId,r.UserId});


            builder.HasIndex(r => new { r.PostId, r.UserId })
                    .IsUnique();


            builder.HasOne<Post>()
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
