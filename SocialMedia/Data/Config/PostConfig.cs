using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.models;

namespace SocialMedia.Data.Config
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(p => p.Author).WithMany(a => a.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

            //builder.Property(e => e.Content).HasColumnType("jsonb");

            builder.OwnsOne(p => p.Content, Contentbuilder =>
            {

                Contentbuilder.ToJson();

                Contentbuilder.OwnsOne(c => c.meta);

                Contentbuilder.OwnsOne(c => c.markdown);
            });

        }
    }
}
