using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.RoleService.Repository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(long id);
        Task<Role> GetRoleUserAsync();
    }
}
