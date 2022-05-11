using System.Text.Json.Serialization;

namespace WorkerManagementAPI.Entities
{
    public class Worker : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? Password { get; set; }

        public long CompanyId { get; set; }
        public virtual Company Company { get; set; } = new Company();
        public virtual List<Technology> Technologies { get; set; } = new List<Technology>();
        public virtual List<Project> Projects { get; set; } = new List<Project>();
    }
}
