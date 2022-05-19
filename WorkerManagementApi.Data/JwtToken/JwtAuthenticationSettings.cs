namespace WorkerManagementAPI.Data.JwtToken
{
    public class JwtAuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
        public string? JwtIssuer { get; set; }
    }
}
