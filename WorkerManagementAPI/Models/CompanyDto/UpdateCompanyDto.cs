using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.CompanyDto
{
    public class UpdateCompanyDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
