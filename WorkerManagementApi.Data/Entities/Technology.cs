﻿using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Data.Entities
{
    public class Technology
    {
        public long Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public TechnologyLevelEnum TechnologyLevel { get; set; } = TechnologyLevelEnum.None;
        public virtual List<User> Users { get; set; } = new List<User>();
        public virtual List<Project> Projects { get; set; } = new List<Project>();
    }
}
