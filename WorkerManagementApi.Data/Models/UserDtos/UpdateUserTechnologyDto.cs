using System.ComponentModel.DataAnnotations;
using WorkerManagementAPI.Data.Models.TechnologyDtos;

namespace WorkerManagementAPI.Data.Models.UserDtos
{
    public class UpdateUserTechnologyDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(40)]
        public string Surname { get; set; } = String.Empty;
        public List<TechnologyDto> TechnologiesDto { get; set; } = new List<TechnologyDto>();
    }
}
