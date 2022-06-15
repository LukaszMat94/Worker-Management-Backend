using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.ExceptionsTemplate;
using WorkerManagementAPI.Services.TokenService.Repository;

namespace WorkerManagementAPI.Services.TokenService.Service
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly JwtAuthenticationSettings _jwtAuthenticationSettings;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenService(ITokenRepository tokenRepository, 
            JwtAuthenticationSettings jwtAuthenticationSettings,
            IDistributedCache cache,
            IHttpContextAccessor contextAccessor)
        {
            _tokenRepository = tokenRepository;
            _jwtAuthenticationSettings = jwtAuthenticationSettings;
            _cache = cache;
            _contextAccessor = contextAccessor;
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

                throw new TokenExpiredException(ExceptionCodeTemplate.BCKND_TOKEN_EXPIRED_UNAUTHORIZED);
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

            RemoveRefreshToken(refreshToken);

            await SaveRefreshTokenAsync(newRefreshToken, user);

            return tokens;
        }

        private void RemoveRefreshToken(RefreshToken refreshToken)
        {
            _tokenRepository.RemoveRefreshToken(refreshToken);
        }

        public async Task RemoveRefreshTokenFromUserByIdAsync(long userId)
        {
            _tokenRepository.RemoveRefreshTokenByUserId(userId);

            await _tokenRepository.SaveChangesAsync();
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
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_TOKEN_NOTFOUND);
            }
        }

        public async Task DeactivateCurrentAccessTokenAsync()
        {
            //string accessToken = GetCurrentAccessToken();

            //await DeactivateAccessTokenAsync(accessToken);
        }

        public string GetCurrentAccessToken()
        {
            string authorization = _contextAccessor.HttpContext.Request.Headers.Authorization;

            string token = GetTokenFromAuthorization(authorization);

            return token;
        }

        private string GetTokenFromAuthorization(string authorization)
        {
            if (authorization == null)
            {
                return String.Empty;
            }

            return authorization.Split(" ").Last();
        }

        private async Task DeactivateAccessTokenAsync(string token)
        {
            await _cache.SetStringAsync(token, "deactivated tokens", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtAuthenticationSettings.JwtAccessExpireMinutes)
            });
        }

        public async Task<bool> IsCurrentAccessTokenActiveAsync()
        {
            string accessToken = GetCurrentAccessToken();

            bool statusAccessToken = await IsAccessTokenActiveAsync(accessToken);

            return statusAccessToken;
        }

        public async Task<bool> IsAccessTokenActiveAsync(string token)
        {
            string cachedToken = await _cache.GetStringAsync(token);

            bool isAccessTokenActive = CheckIfCachedTokenExist(cachedToken);

            return isAccessTokenActive;
        }

        private bool CheckIfCachedTokenExist(string cachedToken)
        {
            if (cachedToken == null)
            {
                return true;
            }

            return false;
        }
    }
}
