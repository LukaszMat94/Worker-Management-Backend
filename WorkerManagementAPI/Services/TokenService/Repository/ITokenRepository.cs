using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Services.TokenService.Repository
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetActiveRefreshTokenAsync(User user);
        void AssignRefreshTokenToUser(RefreshToken refreshToken, User user);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}
