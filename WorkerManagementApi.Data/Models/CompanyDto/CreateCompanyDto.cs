using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Models.CompanyDto
{
    public class CreateCompanyDto
    {   
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
