namespace WorkerManagementAPI.Data.JwtToken
{
    public class JwtAuthenticationSettings
    {
        public string JwtKey { get; set; } = String.Empty;
        public int JwtExpireDays { get; set; }
        public int JwtRefreshExpireDays { get; set; }
        public string? JwtIssuer { get; set; }
    }
}
