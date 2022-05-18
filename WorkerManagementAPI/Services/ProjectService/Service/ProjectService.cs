using AutoMapper;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Services.ProjectService.Repository;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.WorkerService.Repository;

namespace WorkerManagementAPI.Services.ProjectService.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository,
            ITechnologyRepository technologyRepository,
            IWorkerRepository workerRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _technologyRepository = technologyRepository;
            _workerRepository = workerRepository;
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
                throw new NotFoundException("List is empty");
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
                throw new NotFoundException("Project not found");
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
                throw new NotFoundException("Technology not found");
            }
        }

        private void CheckIfRelationProjectTechnologyExist(Project project, Technology technology)
        {
            List<Technology> technology1 = project.Technologies;

            if (project.Technologies.Contains(technology))
            {
                throw new DataDuplicateException("Relation already exist");
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
                throw new NotFoundException("Relation not exist");
            }
        }

        public async Task<UpdateProjectWorkerDto> AssignWorkerToProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await _projectRepository.GetProjectWithMembersByIdAsync(patchProjectWorkerDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            Worker worker = await _workerRepository.GetWorkerByIdAsync(patchProjectWorkerDto.IdWorker);

            CheckIfWorkerEntityIsNull(worker);

            CheckIfRelationProjectWorkerExist(project, worker);

            _projectRepository.AssignWorkerToProject(project, worker);

            await _projectRepository.SaveChangesAsync();

            UpdateProjectWorkerDto updateProjectWorkerDto = _mapper.Map<UpdateProjectWorkerDto>(project);

            return updateProjectWorkerDto;
        }

        private void CheckIfWorkerEntityIsNull(Worker worker)
        {
            if(worker == null)
            {
                throw new NotFoundException("Worker not found");
            }
        }

        private void CheckIfRelationProjectWorkerExist(Project project, Worker worker)
        {
            if (project.Members.Contains(worker))
            {
                throw new DataDuplicateException("Relation already exist");
            }
        }

        public async Task UnassignWorkerFromProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await _projectRepository.GetProjectWithMembersByIdAsync(patchProjectWorkerDto.IdProject);

            CheckIfProjectEntityIsNull(project);

            Worker worker = await _workerRepository.GetWorkerByIdAsync(patchProjectWorkerDto.IdWorker);

            CheckIfWorkerEntityIsNull(worker);

            CheckIfRelationProjectWorkerNonExist(project, worker);

            _projectRepository.UnassignWorkerFromProject(project, worker);

            await _projectRepository.SaveChangesAsync();
        }

        private void CheckIfRelationProjectWorkerNonExist(Project project, Worker worker)
        {
            if (!project.Members.Contains(worker))
            {
                throw new NotFoundException("Relation not exist");
            }
        }
    }
}
