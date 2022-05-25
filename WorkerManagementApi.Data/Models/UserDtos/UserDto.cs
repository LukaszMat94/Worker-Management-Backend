using System.ComponentModel.DataAnnotations;

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
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid email pattern")]
        public string Email { get; set; } = String.Empty;

    }
}
