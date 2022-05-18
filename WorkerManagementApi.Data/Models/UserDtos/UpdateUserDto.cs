using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class UpdateUserDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(40)]
        public string Surname { get; set; } = String.Empty;
    }
}
