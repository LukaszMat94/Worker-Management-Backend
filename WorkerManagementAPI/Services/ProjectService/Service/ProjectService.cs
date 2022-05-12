using AutoMapper;
using WorkerManagementApi.Data.Models.ProjectDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.ProjectDtos;
using WorkerManagementAPI.Services.ProjectService.Repository;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            List<Project> projects = await _projectRepository.GetAllProjectsAsync();

            List<ProjectDto> projectsDto = _mapper.Map<List<ProjectDto>>(projects);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(long id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);

            ProjectDto projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            Project project = _mapper.Map<Project>(createProjectDto);

            Project createdProject = await _projectRepository.CreateProjectAsync(project);

            ProjectDto createdProjectDto = _mapper.Map<ProjectDto>(createdProject);

            return createdProjectDto;
        }

        public async Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto)
        {
            Project project = await _projectRepository.UpdateProjectAsync(projectDto);

            ProjectDto updatedProjectDto = _mapper.Map<ProjectDto>(project);

            return updatedProjectDto;
        }

        public async Task DeleteProjectAsync(long id)
        {
            await _projectRepository.DeleteProjectAsync(id);
        }

        public async Task<UpdateProjectTechnologyDto> AssignTechnologyToProject(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await _projectRepository.AssignTechnologyToProject(patchProjectTechnologyDto);

            UpdateProjectTechnologyDto updateProjectTechnologyDto = _mapper.Map<UpdateProjectTechnologyDto>(project);

            return updateProjectTechnologyDto;
        }
    }
}
