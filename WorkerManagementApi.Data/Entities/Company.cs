namespace WorkerManagementAPI.Data.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = String.Empty;

        public virtual List<Worker>? Workers { get; set; } = new List<Worker>();
    }
}
