using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(long id);
        Task<Project> GetProjectWithTechnologiesByIdAsync(long id);
        Task<Project> GetProjectWithMembersByIdAsync(long id);
        Task<Project> CreateProjectAsync(Project project);
        void DeleteProject(Project project);
        Task SaveChangesAsync();
        void AssignTechnologyToProject(Project project, Technology technology);
        void UnassignTechnologyFromProject(Project project, Technology technology);
        void AssignWorkerToProject(Project project, Worker worker);
        void UnassignWorkerFromProject(Project project, Worker worker);
    }
}
