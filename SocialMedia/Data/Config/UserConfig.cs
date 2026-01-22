using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.models;

namespace SocialMedia.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // This configures the one-to-many relationship.
            // A User (u) has many Posts (p).
            // Each Post (p) has one Author (u).
            // The foreign key on the Post entity is AuthorId.
            builder.HasMany(u => u.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId);


            
        }
    }
}