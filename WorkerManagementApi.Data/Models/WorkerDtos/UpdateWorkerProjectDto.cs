using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Models.ProjectDtos;

namespace WorkerManagementApi.Data.Models.WorkerDtos
{
    public class UpdateWorkerProjectDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        public string Surname { get; set; } = String.Empty;

        public ProjectDto ProjectDto { get; set; } = new ProjectDto();
    }
}
