using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Services.TokenService.Repository;

namespace WorkerManagementAPI.Services.TokenService.Service
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly JwtAuthenticationSettings _jwtAuthenticationSettings;

        public TokenService(ITokenRepository tokenRepository, 
            JwtAuthenticationSettings jwtAuthenticationSettings)
        {
            _tokenRepository = tokenRepository;
            _jwtAuthenticationSettings = jwtAuthenticationSettings;
        }

        public void AssignRefreshTokenToUser(RefreshToken refreshToken, User user)
        {
            _tokenRepository.AssignRefreshTokenToUser(refreshToken, user);
        }

        public async Task<RefreshToken> GetActiveRefreshTokenAsync(User user)
        {
            RefreshToken refreshToken = await _tokenRepository.GetActiveRefreshTokenAsync(user);

            CheckIfRefreshTokenNonExpired(refreshToken);

            return refreshToken;
        }

        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, $"{user.Role.RoleName}")
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthenticationSettings.JwtKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expireDay = DateTime.Now.AddDays(_jwtAuthenticationSettings.JwtExpireDays);

            JwtSecurityToken token = new JwtSecurityToken(_jwtAuthenticationSettings.JwtIssuer,
                _jwtAuthenticationSettings.JwtIssuer,
                claims,
                expires: expireDay,
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateJwtRefreshToken(User user)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(_jwtAuthenticationSettings.JwtRefreshExpireDays),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }

        private async void CheckIfRefreshTokenNonExpired(RefreshToken refreshToken)
        {
            if (refreshToken.IsExpired)
            {
                refreshToken.TokenStatus = false;
                await _tokenRepository.SaveChangesAsync();

                throw new SecurityTokenExpiredException("Refresh token expired!");
            }
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken, User user)
        {
            _tokenRepository.AssignRefreshTokenToUser(refreshToken, user);

            await _tokenRepository.SaveChangesAsync();
        }
    }
}
