using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Services.TokenService.Service
{
    public interface ITokenService
    {
        Task<RefreshToken> GetActiveRefreshTokenAsync(User user);
        void AssignRefreshTokenToUser(RefreshToken refreshToken, User user);
        RefreshToken GenerateJwtRefreshToken(User user);
        string GenerateJwtAccessToken(User user);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken, User user);
        Task<Dictionary<string, string>> RefreshTokensAsync(User user, RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByTokenAndUserIdAsync(long userId, string refreshToken);
        Task CheckIfRefreshTokenNonExpiredAsync(RefreshToken refreshToken);
        string GetCurrentAccessToken();
        Task<bool> IsCurrentAccessTokenActiveAsync();
        Task<bool> IsAccessTokenActiveAsync(string token);
        Task DeactivateCurrentAccessTokenAsync();
    }
}
