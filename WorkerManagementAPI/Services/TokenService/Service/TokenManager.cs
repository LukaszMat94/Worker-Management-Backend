using Microsoft.Extensions.Caching.Distributed;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Services.TokenService.Repository;

namespace WorkerManagementAPI.Services.TokenService.Service
{
    public class TokenManager : ITokenManager
    {
        private readonly JwtAuthenticationSettings _jwtAuthenticationSettings;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenManager(JwtAuthenticationSettings jwtAuthenticationSettings,
            IDistributedCache cache,
            IHttpContextAccessor contextAccessor)
        {
            _jwtAuthenticationSettings = jwtAuthenticationSettings;
            _cache = cache;
            _contextAccessor = contextAccessor;
        }

        private static string GetKeyFromCache(string token)
        {
            return token;
        }

        public async Task DeactivateAccessTokenAsync(string token)
        {
            await _cache.SetStringAsync(GetKeyFromCache(token), "deactivated tokens", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtAuthenticationSettings.JwtAccessExpireMinutes)
            });
        }

        public string GetCurrentAccessToken()
        {
            string authorization = _contextAccessor.HttpContext.Request.Headers.Authorization;

            if (authorization == null)
            {
                return String.Empty;
            }

            return authorization.Split(" ").Last();
        }

        public async Task<bool> IsCurrentAccessTokenActiveAsync()
        {
            return await IsAccessTokenActiveAsync(GetCurrentAccessToken());
        }

        public async Task<bool> IsAccessTokenActiveAsync(string token)
        {
            string cachedToken = await _cache.GetStringAsync(token);

            if (cachedToken == null)
            {
                return true;
            }

            return false;
        }

        public async Task DeactivateCurrentAccessTokenAsync()
        {
            await DeactivateAccessTokenAsync(GetCurrentAccessToken());
        }
    }
}
