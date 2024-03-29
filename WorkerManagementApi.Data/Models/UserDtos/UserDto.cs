﻿using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class UserDto
    {
        public long Id { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(40)]
        public string Surname { get; set; } = String.Empty;

        [Required]
        [MaxLength(35)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = String.Empty;

    }
}
