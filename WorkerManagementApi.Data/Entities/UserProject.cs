namespace WorkerManagementAPI.Data.Entities
{
    public class UserProject
    {
        public virtual User User { get; set; } = new User();
        public long UserId { get; set; }
        public virtual Project Project { get; set; } = new Project();
        public long ProjectId { get; set; }
    }
}
