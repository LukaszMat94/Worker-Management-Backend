using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class LoginUserDto
    {
        [Required]
        [MaxLength(35)]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid email pattern")]
        public string Email { get; set; } = String.Empty;

        [Required]
        [MaxLength(30)]
        public string Password { get; set; } = String.Empty;
    }
}
