using SocialMedia.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<bool> GetDataAsync(string email);
    }
}
