using WorkerManagementAPI.Data.Models.WorkerDtos;

namespace WorkerManagementAPI.Data.Models.CompanyDtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WorkerDto> Workers { get; set; } = new List<WorkerDto>();
    }
}
