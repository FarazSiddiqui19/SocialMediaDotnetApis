namespace SocialMedia.DTO.Users
{
    public class UserLoginDTO
    {
        public Guid UserId { get; set; }

        public required string Password { get; set; }
    }
}
