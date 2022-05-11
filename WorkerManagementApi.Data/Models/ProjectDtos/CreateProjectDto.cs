using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.ProjectDtos
{
    public class CreateProjectDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
    }
}
