using SocialMedia.models;
using SocialMedia.models.DTO.Users;

namespace SocialMedia.mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this AddUsersDTO dto)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username
            };
        }

        public static VeiwUsersDTO ToDTO(this User user)
        {
            return new VeiwUsersDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}
