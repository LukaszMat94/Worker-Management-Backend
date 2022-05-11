using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.CompanyDtos
{
    public class UpdateCompanyDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
