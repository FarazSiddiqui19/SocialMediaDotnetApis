namespace SocialMedia.DTO.Users
{
    public class CreateUserDTO
    {
        public required string Username { get; set; }
        public string Password { get; set; } = string.Empty;

     

        public string Email { get; set; }   
    }
}
