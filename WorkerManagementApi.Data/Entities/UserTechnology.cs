namespace WorkerManagementAPI.Data.Entities
{
    public class UserTechnology
    {
        public virtual User User { get; set; } = new User();
        public long UserId { get; set; }
        public virtual Technology Technology { get; set; } = new Technology();
        public long TechnologyId { get; set; }
    }
}
