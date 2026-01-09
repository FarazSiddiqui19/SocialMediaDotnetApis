using SocialMedia.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface ITokenGeneratorService
    {
        Task<TokenDTO> GenerateTokenAsync(Guid userId,string Username);
    }
}
