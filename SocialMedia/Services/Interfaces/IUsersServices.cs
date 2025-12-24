using SocialMedia.models.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<VeiwUsersDTO> CreateUserAsync(AddUsersDTO dto);
        Task<List<VeiwUsersDTO>> GetAllUsersAsync(string? Username);

       
        Task<VeiwUsersDTO?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, AddUsersDTO dto);
    }
}
