using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.ProjectDto
{
    public class CreateProjectDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
    }
}
