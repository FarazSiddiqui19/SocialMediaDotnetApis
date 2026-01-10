using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;

namespace SocialMedia.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<UserResponseDto> CreateUserAsync(CreateUserDTO dto);
        Task<PagedResults<UserResponseDto>> GetAllUsersAsync(UsersFilter filter);

        Task<UserLoginResposeDTO?> LoginAsync(UserLoginDTO LoginRequest);
        Task<UserResponseDto?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, CreateUserDTO dto);
    }
}
