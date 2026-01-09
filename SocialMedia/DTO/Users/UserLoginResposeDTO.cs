namespace SocialMedia.DTO.Users
{
    public class UserLoginResposeDTO
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Token { get; set; }

        public required DateTime TokenExpiry { get; set; }
    }
}
