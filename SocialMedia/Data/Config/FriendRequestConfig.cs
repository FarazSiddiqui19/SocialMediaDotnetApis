using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.models;
using SocialMedia.Models;

namespace SocialMedia.Data.Config
{
    public class FriendRequestConfig : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            
            builder.HasKey(f => new { f.SenderId, f.RecieverId });

          
            builder.HasOne(f => f.Sender)
                .WithMany() 
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict); 

          
           
          
            builder.HasOne(f => f.Reciever)
                .WithMany(u => u.Requests)
                .HasForeignKey(f => f.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);        
        }
    }
}
