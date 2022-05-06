using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.ProjectDto;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(long id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(ProjectDto project);
        Task DeleteProjectAsync(long id);
    }
}
