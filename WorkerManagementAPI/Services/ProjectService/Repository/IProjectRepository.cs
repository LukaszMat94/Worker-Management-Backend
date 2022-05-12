using WorkerManagementApi.Data.Models.ProjectDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.ProjectDtos;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(long id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(ProjectDto project);
        Task<bool> DeleteProjectAsync(long id);
        Task<Project> AssignTechnologyToProject(PatchProjectTechnologyDto patchProjectTechnologyDto);
    }
}
