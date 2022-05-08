namespace WorkerManagementAPI.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = String.Empty;

        public virtual List<Worker> Members { get; set; } = new List<Worker>();
        public virtual List<Technology> Technologies { get; set; } = new List<Technology>();
    }
}
