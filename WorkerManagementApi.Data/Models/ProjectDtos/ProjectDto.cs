﻿using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.ProjectDtos
{
    public class ProjectDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

    }
}
