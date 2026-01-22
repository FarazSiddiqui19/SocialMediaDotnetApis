using SocialMedia.DTO.Users;
using SocialMedia.models;

namespace SocialMedia.mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this CreateUserDTO dto, byte[] passwordBytes)
        {
            return new User
            {
                Username = dto.Username,
                HashedPassword = passwordBytes,
                Email = dto.Email,
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
