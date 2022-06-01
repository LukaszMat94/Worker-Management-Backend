using AutoMapper;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Services.ProjectService.Repository;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.UserService.Repository;
using WorkerManagementAPI.ExceptionsTemplate;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository,
            ITechnologyRepository technologyRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _technologyRepository = technologyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            List<Project> projects = await _projectRepository.GetAllProjectsAsync();

            CheckIfListIsNull(projects);

            List<ProjectDto> projectsDto = _mapper.Map<List<ProjectDto>>(projects);

            return projectsDto;
        }

        private void CheckIfListIsNull(List<Project> projects)
        {
            if(projects == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_PROJECT_LIST_NOTFOUND);
            }         
        }

        public async Task<ProjectDto> GetProjectByIdAsync(long id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);

            CheckIfProjectEntityIsNull(project);

            ProjectDto projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        private void CheckIfProjectEntityIsNull(Project project)
        {
            if(project == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_PROJECT_NOTFOUND);
            }
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            Project project = _mapper.Map<Project>(createProjectDto);

            Project createdProject = await _projectRepository.CreateProjectAsync(project);

            await _projectRepository.SaveChangesAsync();

            ProjectDto addedProjectDto = _mapper.Map<ProjectDto>(createdProject);

            return addedProjectDto;
        }

        public async Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectDto.Id);

            CheckIfProjectEntityIsNull(project);

            UpdateProjectProperties(project, projectDto);

            await _projectRepository.SaveChangesAsync();

            ProjectDto updatedProjectDto = _mapper.Map<ProjectDto>(project);

            return updatedProjectDto;
        }

        private void UpdateProjectProperties(Project project, ProjectDto projectDto)
        {
            project.Name = projectDto.Name;
        }

        public async Task DeleteProjectAsync(long id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);

            CheckIfProjectEntityIsNull(project);

            _projectRepository.DeleteProject(project);

            await _projectRepository.SaveChangesAsync();
        }

        public async Task<UpdateProjectTechnologyDto> AssignTechnologyToProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await _projectRepository.GetProjectWithTechnologiesByIdAsync(patchProjectTechnologyDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchProjectTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationProjectTechnologyExist(project, technology);

            _projectRepository.AssignTechnologyToProject(project, technology);

            await _projectRepository.SaveChangesAsync();

            UpdateProjectTechnologyDto updateProjectTechnologyDto = _mapper.Map<UpdateProjectTechnologyDto>(project);

            return updateProjectTechnologyDto;
        }

        private void CheckIfTechnologyEntityIsNull(Technology technology)
        {
            if(technology == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_TECHNOLOGY_NOTFOUND);
            }
        }

        private void CheckIfRelationProjectTechnologyExist(Project project, Technology technology)
        {
            List<Technology> technology1 = project.Technologies;

            if (project.Technologies.Contains(technology))
            {
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_RELATION_CONFLICT);
            }
        }

        public async Task UnassignTechnologyFromProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await _projectRepository.GetProjectWithTechnologiesByIdAsync(patchProjectTechnologyDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchProjectTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationProjectTechnologyNonExist(project, technology);

            _projectRepository.UnassignTechnologyFromProject(project, technology);

            await _projectRepository.SaveChangesAsync();
        }

        private void CheckIfRelationProjectTechnologyNonExist(Project project, Technology technology)
        {
            if (!project.Technologies.Contains(technology))
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_RELATION_NOTFOUND);
            }
        }

        public async Task<UpdateProjectUserDto> AssignUserToProjectAsync(PatchProjectUserDto patchProjectUserDto)
        {
            Project project = await _projectRepository.GetProjectWithUsersByIdAsync(patchProjectUserDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            User user = await _userRepository.GetUserByIdAsync(patchProjectUserDto.IdUser);

            CheckIfUserEntityIsNull(user);

            CheckIfRelationProjectUserExist(project, user);

            _projectRepository.AssignUserToProject(project, user);

            await _projectRepository.SaveChangesAsync();

            UpdateProjectUserDto updateProjectUserDto = _mapper.Map<UpdateProjectUserDto>(project);

            return updateProjectUserDto;
        }

        private void CheckIfUserEntityIsNull(User user)
        {
            if(user == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_USER_NOTFOUND);
            }
        }

        private void CheckIfRelationProjectUserExist(Project project, User user)
        {
            if (project.Users.Contains(user))
            {
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_RELATION_CONFLICT);
            }
        }

        public async Task UnassignUserFromProjectAsync(PatchProjectUserDto patchProjectUserDto)
        {
            Project project = await _projectRepository.GetProjectWithUsersByIdAsync(patchProjectUserDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            User user = await _userRepository.GetUserByIdAsync(patchProjectUserDto.IdUser);

            CheckIfUserEntityIsNull(user);

            CheckIfRelationProjectUserNonExist(project, user);

            _projectRepository.UnassignUserFromProject(project, user);

            await _projectRepository.SaveChangesAsync();
        }

        private void CheckIfRelationProjectUserNonExist(Project project, User user)
        {
            if (!project.Users.Contains(user))
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_RELATION_NOTFOUND);
            }
        }
    }
}
