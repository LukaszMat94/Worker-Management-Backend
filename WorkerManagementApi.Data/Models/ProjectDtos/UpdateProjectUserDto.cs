﻿using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.Data.Models.ProjectDtos
{
    public class UpdateProjectUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<UserDto> UsersDto { get; set; } = new List<UserDto>();
    }
}
