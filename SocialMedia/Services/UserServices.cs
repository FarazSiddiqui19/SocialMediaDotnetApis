using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Data.Repository.Interfaces;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.mappers;
using SocialMedia.models;
using SocialMedia.DTO.Posts;
using SocialMedia.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Diagnostics;

namespace SocialMedia.Services
{
    public class UserServices : IUsersServices
    {
        private readonly IUserRepository  _userRepository;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IConfiguration _config;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IEmailVerificationService _emailVerificationService;

        public UserServices(IUserRepository userRepository,IConfiguration config,
                            ITokenGeneratorService tokenGeneratorService,
                            IPasswordHasherService passwordHasherService,
                            IEmailVerificationService emailVerificationService) {
            _userRepository = userRepository;
            _tokenGeneratorService = tokenGeneratorService;
            _config = config;
            _passwordHasherService = passwordHasherService;
            _emailVerificationService = emailVerificationService;
          
        }

       
        public async Task<UserResponseDto> CreateUserAsync(CreateUserDTO dto)
        {
           

           byte[] hashedPassword = _passwordHasherService.HashPassword(dto.Password);
            
            User user = dto.ToEntity(hashedPassword);

            await _userRepository.AddUserAsync(user);
            return user.ToDTO();

        }

       

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            
            User? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUserAsync(user);
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
            User? user  = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return user.ToDTO();
        }

        public async Task<bool> UpdateUserAsync(Guid id, CreateUserDTO dto)
        {
            User? existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Username = dto.Username;
          

            existingUser.HashedPassword = _passwordHasherService.HashPassword(dto.Password);

            await _userRepository.UpdateUserAsync(existingUser);

            return true;
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
