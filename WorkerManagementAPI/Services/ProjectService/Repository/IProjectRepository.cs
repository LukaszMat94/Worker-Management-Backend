using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(long id);
        Task<Project> GetProjectWithTechnologiesByIdAsync(long id);
        Task<Project> GetProjectWithUsersByIdAsync(long id);
        Task<Project> CreateProjectAsync(Project project);
        void DeleteProject(Project project);
        Task SaveChangesAsync();
        void AssignTechnologyToProject(Project project, Technology technology);
        void UnassignTechnologyFromProject(Project project, Technology technology);
        void AssignUserToProject(Project project, User user);
        void UnassignUserFromProject(Project project, User user);
    }
}
