using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Data.Entities
{
    public class Role
    {
        public long Id { get; set; }
        public RoleEnum RoleName { get; set; } = RoleEnum.USER;
    }
}
