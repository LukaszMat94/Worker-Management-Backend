using WorkerManagementAPI.Data.Models.WorkerDtos;

namespace WorkerManagementAPI.Data.Models.ProjectDtos
{
    public class UpdateProjectWorkerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public WorkerDto? WorkerDto { get; set; }
    }
}
