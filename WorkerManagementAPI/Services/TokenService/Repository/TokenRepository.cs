using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Services.TokenService.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public TokenRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> GetActiveRefreshTokenAsync(User user)
        { 
            RefreshToken token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id && t.TokenStatus == true);

            return token;
        }

        public void AssignRefreshTokenToUser(RefreshToken refreshToken, User user)
        {
            user.RefreshToken = refreshToken;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.Remove(refreshToken);
        }

        public void RemoveRefreshTokenByUserId(long userId)
        {
            _dbContext.RefreshTokens.RemoveRange(_dbContext.RefreshTokens.Where(token => token.UserId == userId));
        }

        public async Task<RefreshToken> GetRefreshTokenByTokenAndUserIdAsync(long userId, string userRefreshToken)
        {
            RefreshToken refreshToken = await _dbContext.RefreshTokens.Where(t => t.UserId == userId && t.Token == userRefreshToken).FirstOrDefaultAsync();

            return refreshToken;
        }
    }
}
