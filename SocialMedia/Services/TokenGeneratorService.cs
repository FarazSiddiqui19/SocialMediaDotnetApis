using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.DTO;
using SocialMedia.DTO.Users;
using SocialMedia.models;
using SocialMedia.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {

       
        private readonly JWTConfig _jwtConfig;

        public TokenGeneratorService(IOptions<JWTConfig> JwtOptions)
        {
            _jwtConfig = JwtOptions.Value;
    }
    
        public async Task<TokenDTO> GenerateTokenAsync(Guid userId, string Username) 
        {


            List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Name, Username)
                    };

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _jwtConfig.key
            ));

            string issuer = _jwtConfig.Issuer;
            string audience = _jwtConfig.Audience;
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int ExpiryInMinutes = _jwtConfig.DurationInMinutes;
            DateTime ExpiryDuration = DateTime.UtcNow.AddMinutes(ExpiryInMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                claims: claims,
                                expires: ExpiryDuration,
                                signingCredentials: credentials
                    );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDTO
            {
                Token = jwtToken,
                TokenExpiryTime = ExpiryDuration
            };
        }
    }
}
