namespace SocialMedia.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        byte[] HashPassword(string password);
        bool VerifyPassword(string password, byte[] hashedPassword);

      
    }
}
