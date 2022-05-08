using WorkerManagementAPI.Entities;

namespace WorkerManagementAPI.Models.CompanyDto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Worker> Workers { get; set; } = new List<Worker>();

    }
}
