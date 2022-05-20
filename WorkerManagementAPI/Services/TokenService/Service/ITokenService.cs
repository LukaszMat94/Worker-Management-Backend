using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Services.TokenService.Service
{
    public interface ITokenService
    {
        Task<RefreshToken> GetActiveRefreshTokenAsync(User user);
        void AssignRefreshTokenToUser(RefreshToken refreshToken, User user);
        RefreshToken GenerateJwtRefreshToken(User user);
        string GenerateJwtToken(User user);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken, User user);
    }
}
