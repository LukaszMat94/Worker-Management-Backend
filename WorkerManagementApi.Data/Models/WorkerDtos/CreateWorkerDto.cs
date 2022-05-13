using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.WorkerDtos
{
    public class CreateWorkerDto
    { 
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
