using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Models.WorkerDtos;

namespace WorkerManagementApi.Data.Models.ProjectDtos
{
    public class UpdateProjectWorkerDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        public WorkerDto Worker { get; set; } = new WorkerDto();
    }
}
