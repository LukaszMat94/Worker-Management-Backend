using WorkerManagementAPI.Entities.Enums;

namespace WorkerManagementAPI.Entities
{
    public class Technology
    {
        public long Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public TechnologyLevelEnum TechnologyLevel { get; set; } = TechnologyLevelEnum.None;
        public virtual List<Worker> Workers { get; set; } = new List<Worker>();
        public virtual List<Project> Projects { get; set; } = new List<Project>();
    }
}
