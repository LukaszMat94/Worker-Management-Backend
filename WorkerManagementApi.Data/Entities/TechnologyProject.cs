namespace WorkerManagementAPI.Data.Entities
{
    public class TechnologyProject
    {
        public virtual Project Project { get; set; } = new Project();
        public long ProjectId { get; set; }
        public virtual Technology Technology { get; set; } = new Technology();
        public long TechnologyId { get; set; }
    }
}
