namespace WorkerManagementAPI.Data.Models.RefreshTokenDtos
{
    public class RefreshTokenDto
    {
        public long UserId { get; set; }
        public string Token { get; set; } = String.Empty;
    }
}
