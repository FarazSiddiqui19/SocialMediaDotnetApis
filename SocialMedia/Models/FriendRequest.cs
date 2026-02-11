using SocialMedia.models;

namespace SocialMedia.Models
{
  
    public class FriendRequest 
    {
       
        
        public Guid SenderId { get; set; }
        public virtual User? Sender { get; set; }

        public Guid RecieverId { get; set; }
        public virtual User? Reciever { get; set; }

        public Status status { get; set; }
    }


    public enum Status 
    {
        Pending = 0,
        Accepted =1,
        Rejected=2
     
    
    }
}
