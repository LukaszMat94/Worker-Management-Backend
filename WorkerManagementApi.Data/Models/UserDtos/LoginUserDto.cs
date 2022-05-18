using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class LoginUserDto
    {
        [Required]
        [MaxLength(35)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = String.Empty;

        public string? Password { get; set; }
    }
}
