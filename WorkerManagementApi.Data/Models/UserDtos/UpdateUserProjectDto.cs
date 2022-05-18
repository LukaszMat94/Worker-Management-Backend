using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Data.Models.ProjectDtos;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class UpdateUserProjectDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(40)]
        public string Surname { get; set; } = String.Empty;
        public ProjectDto ProjectDto { get; set; } = new ProjectDto();
    }
}
