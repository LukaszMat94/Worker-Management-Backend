using WorkerManagementAPI.Models.ProjectDto;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(long id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto);
        Task DeleteProjectAsync(long id);
    }
}
