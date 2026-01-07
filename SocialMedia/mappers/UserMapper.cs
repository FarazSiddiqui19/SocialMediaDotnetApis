using SocialMedia.models;
using SocialMedia.models.DTO.Users;

namespace SocialMedia.mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this CreateUserDTO dto)
        {
            return new User
            {
                Username = dto.Username
            };
        }

        public static UserResponseDto ToDTO(this User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}
