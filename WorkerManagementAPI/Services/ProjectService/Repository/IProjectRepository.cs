using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(long id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(ProjectDto project);
        Task DeleteProjectAsync(long id);
        Task<Project> AssignTechnologyToProject(PatchProjectTechnologyDto patchProjectTechnologyDto);
        Task UnassignTechnologyFromProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto);
        Task<Project> AssignWorkerToProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto);
        Task UnassignWorkerFromProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto);
    }
}
