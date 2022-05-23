using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Services.RoleService.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public RoleRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetRoleByIdAsync(long id)
        {
            Role role = await _dbContext.Roles
                .FindAsync(id);

            return role;
        }

        public async Task<Role> GetRoleUserAsync()
        {
            Role role = await _dbContext.Roles
                .FirstAsync(r => r.RoleName.Equals(RoleEnum.USER));

            return role;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            List<Role> roles = await _dbContext.Roles
                .ToListAsync();

            return roles;
        }
    }
}
