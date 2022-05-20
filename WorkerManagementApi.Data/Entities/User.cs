using WorkerManagementAPI.Data.Entities.Enums;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Data.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? Password { get; set; }
        public long? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public virtual List<Technology>? Technologies { get; set; } = new List<Technology>();
        public virtual List<Project>? Projects { get; set; } = new List<Project>();
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
        public AccountStatusEnum AccountStatus { get; set; } = AccountStatusEnum.INACTIVE;
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken> ();
    }
}