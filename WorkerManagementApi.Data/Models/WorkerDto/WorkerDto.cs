using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.WorkerDto
{
    public class WorkerDto
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
        public string Email { get; set; } = String.Empty;

    }
}
