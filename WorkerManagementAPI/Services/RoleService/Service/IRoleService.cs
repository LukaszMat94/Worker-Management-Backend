using WorkerManagementAPI.Data.Models.RoleDtos;

namespace WorkerManagementAPI.Services.RoleService.Service
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(long id);
    }
}
