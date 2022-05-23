using WorkerManagementAPI.Data.Models.ProjectDtos;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(long id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto);
        Task DeleteProjectAsync(long id);
        Task<UpdateProjectTechnologyDto> AssignTechnologyToProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto);
        Task UnassignTechnologyFromProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto);
        Task<UpdateProjectUserDto> AssignUserToProjectAsync(PatchProjectUserDto patchProjectUserDto);
        Task UnassignUserFromProjectAsync(PatchProjectUserDto patchProjectUserDto);
    }
}
