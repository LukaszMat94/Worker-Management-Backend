namespace WorkerManagementAPI.Data.JwtToken
{
    public class JwtAuthenticationSettings
    {
        public string JwtKey { get; set; } = String.Empty;
        public int JwtAccessExpireMinutes { get; set; }
        public int JwtRefreshExpireDays { get; set; }
        public string? JwtIssuer { get; set; }
    }
}
