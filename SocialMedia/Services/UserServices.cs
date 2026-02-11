using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class UserServices : IUsersServices
    {
        private readonly IUserRepository  _userRepository;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserServices(IUserRepository userRepository,
                            ITokenGeneratorService tokenGeneratorService,
                            IPasswordHasherService passwordHasherService) 
        {
            _userRepository = userRepository;
            _tokenGeneratorService = tokenGeneratorService;
            _passwordHasherService = passwordHasherService;
            
        }

       
        public async Task<UserResponseDto> CreateUserAsync(CreateUserDTO dto)
        {
           

           byte[] hashedPassword = _passwordHasherService.HashPassword(dto.Password);
            
            User user = dto.ToEntity(hashedPassword);

            await _userRepository.AddAsync(user);
            return user.ToDTO();

        }

       

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            
            User? user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<PagedResults<UserResponseDto>> GetAllUsersAsync(UsersFilter filter)
        {
          
            string? Username = filter.Username;
            int page = filter.page;
            int pageSize = filter.pageSize;
            SortOrder orderby = filter.orderby;
          PagedResults<UserResponseDto> UsersList = await _userRepository
                                                    .GetAllUsersAsync(Username, pagesize:pageSize, page:page, orderby);

          

            return UsersList;

        }

      

        public async Task<Guid?> GetUserIdByEmailAsync(string? email)
        {
            Guid? userId = await _userRepository.GetUserByIdEmailAsync(email);
            if (userId == null)
                return null;
            return userId;
        }



        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            User? user  = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return user.ToDTO();
        }

        public async Task<bool> UpdateUserAsync(Guid id, CreateUserDTO dto)
        {
            User? existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Username = dto.Username;
          

            existingUser.HashedPassword = _passwordHasherService.HashPassword(dto.Password);

            await _userRepository.UpdateAsync(existingUser);

            return true;
        }


        public async Task<bool> SendFriendRequest(Guid SenderId, Guid RecieverId)
        {
            var existingRequest = await _userRepository.FriendRequestExists(SenderId, RecieverId);

            if (existingRequest)
                return false;

            return await _userRepository.AddFriendRequest(SenderId, RecieverId);
        }

        public async Task<bool> RespondToFriendRequest(FriendRequest request)
        {
            var existingRequest = await _userRepository.FriendRequestExists(request.SenderId, request.RecieverId);

            if (existingRequest)
                return false;

            return await _userRepository.UpdateFriendRequest(request);
        }

        public async Task<PagedResults<FriendRequest>?> GetAllFriendRequests(Guid LoggedInUser, int pageSize, int page)
        {
            return await _userRepository.GetAllFriendRequests(LoggedInUser, pageSize, page);
        }

        public async Task<UserLoginResposeDTO?> LoginAsync(UserLoginDTO LoginRequest)
        {
            User? user = await _userRepository.GetUserByEmailAsync(LoginRequest.Email);
            if (user == null)
                return null;
          
            bool verifyPassword = _passwordHasherService.VerifyPassword(LoginRequest.Password, user.HashedPassword);

            if (verifyPassword == false)
            {
                return null;
            }

            TokenDTO? jwtToken = await _tokenGeneratorService.GenerateTokenAsync(user.Id,user.Username);

            if (String.IsNullOrEmpty(jwtToken.Token))
                return null;

            return new UserLoginResposeDTO
            {
                Id = user.Id,
                Username = user.Username,
                Token = jwtToken.Token,
                TokenExpiry = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.TokenExpiryTime, TimeZoneInfo.Local)
            };
        }

    }
}
