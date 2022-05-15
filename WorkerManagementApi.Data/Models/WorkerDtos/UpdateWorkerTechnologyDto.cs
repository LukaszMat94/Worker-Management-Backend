using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Data.Models.TechnologyDtos;

namespace WorkerManagementAPI.Data.Models.WorkerDtos
{
    public class UpdateWorkerTechnologyDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(40)]
        public string Surname { get; set; } = String.Empty;
        public TechnologyDto TechnologyLevelDto { get; set; } = new TechnologyDto();
    }
}
