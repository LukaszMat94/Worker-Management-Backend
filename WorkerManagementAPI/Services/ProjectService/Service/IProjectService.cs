using WorkerManagementApi.Data.Models.ProjectDtos;
using WorkerManagementAPI.Models.ProjectDtos;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(long id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto);
        Task DeleteProjectAsync(long id);
        Task<UpdateProjectTechnologyDto> AssignTechnologyToProject(PatchProjectTechnologyDto patchProjectTechnologyDto);
    }
}
