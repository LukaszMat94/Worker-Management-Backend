namespace WorkerManagementAPI.Services.TokenService.Repository
{
    public interface ITokenManager
    {
        Task DeactivateAccessTokenAsync(string token);
        string GetCurrentAccessToken();
        Task<bool> IsCurrentAccessTokenActiveAsync();
        Task<bool> IsAccessTokenActiveAsync(string token);
        Task DeactivateCurrentAccessTokenAsync();
    }
}
