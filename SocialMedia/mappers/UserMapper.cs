using SocialMedia.models;
using SocialMedia.models.DTO.Users;

namespace SocialMedia.mappers
{
    public static class UserMapper
    {
        public static Users ToUser(this AddUsersDTO dto)
        {
            return new Users
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username
            };
        }

        public static VeiwUsersDTO Toveiw(this Users user)
        {
            return new VeiwUsersDTO
            {
                Username = user.Username
            };
        }
    }
}
