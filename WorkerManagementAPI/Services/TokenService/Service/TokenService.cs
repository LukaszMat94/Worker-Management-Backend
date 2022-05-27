using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Exceptions;
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

            await CheckIfRefreshTokenNonExpiredAsync(refreshToken);

            return refreshToken;
        }

        public string GenerateJwtAccessToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("nameidentifier", user.Id.ToString()),
                new Claim("name", $"{user.Name} {user.Surname}"),
                new Claim("role", $"{user.Role.RoleName}")
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthenticationSettings.JwtKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expireMinutes = DateTime.UtcNow.AddMinutes(_jwtAuthenticationSettings.JwtAccessExpireMinutes);

            JwtSecurityToken token = new JwtSecurityToken(_jwtAuthenticationSettings.JwtIssuer,
                _jwtAuthenticationSettings.JwtIssuer,
                claims,
                expires: expireMinutes,
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateJwtRefreshToken(User user)
        {
            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(_jwtAuthenticationSettings.JwtRefreshExpireDays),
                Created = DateTime.UtcNow
            };

            return refreshToken;
        }

        public async Task CheckIfRefreshTokenNonExpiredAsync(RefreshToken refreshToken)
        {
            if (refreshToken.IsExpired)
            {
                refreshToken.TokenStatus = false;
                await _tokenRepository.SaveChangesAsync();

                throw new TokenExpiredException("Refresh token expired!");
            }
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken, User user)
        {
            _tokenRepository.AssignRefreshTokenToUser(refreshToken, user);

            await _tokenRepository.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> RefreshTokensAsync(User user, RefreshToken refreshToken)
        {
            await CheckIfRefreshTokenNonExpiredAsync(refreshToken);

            string newAccessToken = GenerateJwtAccessToken(user);
            RefreshToken newRefreshToken = GenerateJwtRefreshToken(user);

            Dictionary<string, string> tokens = new()
            {
                { "accessToken", newAccessToken },
                { "refreshToken", newRefreshToken.Token }
            };

            RemoveOldRefreshToken(refreshToken);

            await SaveRefreshTokenAsync(newRefreshToken, user);

            return tokens;
        }

        private void RemoveOldRefreshToken(RefreshToken refreshToken)
        {
            _tokenRepository.RemoveRefreshToken(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenByTokenAndUserIdAsync(long userId, string refreshToken)
        {
            RefreshToken refreshTokenFromDB = await _tokenRepository.GetRefreshTokenByTokenAndUserIdAsync(userId, refreshToken);

            CheckIfTokenFromDBIsNull(refreshTokenFromDB);

            return refreshTokenFromDB;
        }

        private void CheckIfTokenFromDBIsNull(RefreshToken refreshTokenFromDB)
        {
            if(refreshTokenFromDB == null)
            {
                throw new NotFoundException("Token not found");
            }
        }
    }
}
