namespace WorkerManagementAPI.Data.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = String.Empty;

        public virtual List<User> Users{ get; set; } = new List<User>();
    }
}
