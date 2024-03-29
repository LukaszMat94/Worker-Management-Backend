﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Data.Models.RoleDtos;
using WorkerManagementAPI.Services.RoleService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            List<RoleDto> roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] long id)
        {
            RoleDto role = await _roleService.GetRoleByIdAsync(id);
            return Ok(role);
        }
    }
}
