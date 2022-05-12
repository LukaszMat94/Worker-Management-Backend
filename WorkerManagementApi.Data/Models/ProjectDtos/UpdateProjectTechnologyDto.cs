using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Models.TechnologyDtos;

namespace WorkerManagementApi.Data.Models.ProjectDtos
{
    public class UpdateProjectTechnologyDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        public TechnologyDto Technology { get; set; } = new TechnologyDto();
    }
}
