using SocialMedia.models.DTO;
using SocialMedia.models.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<VeiwUsersDTO> CreateUserAsync(AddUsersDTO dto);
        Task<PagedResults<VeiwUsersDTO>> GetAllUsersAsync(string? Username, int page, int pageSize,SortingOrder ord);

       
        Task<VeiwUsersDTO?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, AddUsersDTO dto);
    }
}
