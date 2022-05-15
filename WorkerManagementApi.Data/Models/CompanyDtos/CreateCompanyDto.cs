using System.ComponentModel.DataAnnotations;

namespace WorkerManagementAPI.Data.Models.CompanyDtos
{
    public class CreateCompanyDto
    {   
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
