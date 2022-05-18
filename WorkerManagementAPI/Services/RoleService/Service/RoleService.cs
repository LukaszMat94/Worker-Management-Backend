using AutoMapper;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.RoleDtos;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.RoleService.Repository;

namespace WorkerManagementAPI.Services.RoleService.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, 
            IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto> GetRoleByIdAsync(long id)
        {
            Role role = await _roleRepository.GetRoleByIdAsync(id);

            CheckIfRoleEntityIsNull(role);

            RoleDto roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        private void CheckIfRoleEntityIsNull(Role role)
        {
            if(role == null)
            {
                throw new NotFoundException("Role not found");
            }
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            List<Role> roles = await _roleRepository.GetRolesAsync();

            CheckIfListIsNull(roles);

            List<RoleDto> rolesDto = _mapper.Map<List<RoleDto>>(roles);

            return rolesDto;
        }

        private void CheckIfListIsNull(List<Role> roles)
        {
            if(roles.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }
        }
    }
}
