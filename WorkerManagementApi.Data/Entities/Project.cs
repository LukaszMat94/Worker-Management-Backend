namespace WorkerManagementAPI.Data.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = String.Empty;

        public virtual List<User> Users { get; set; } = new List<User>();
        public virtual List<Technology> Technologies { get; set; } = new List<Technology>();
    }
}
