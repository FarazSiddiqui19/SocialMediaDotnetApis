using SocialMedia.models.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<VeiwUsersDTO> CreateUserAsync(AddUsersDTO dto);
        Task<IEnumerable<VeiwUsersDTO>> GetAllUsersAsync();
        Task<VeiwUsersDTO?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
    }
}
