using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Data.JwtToken
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = new User();
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool TokenStatus { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool isActive => !IsExpired;
    }
}
