using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public ProjectRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            List<Project> projects = await _dbContext.Projects.ToListAsync();

            CheckIfListIsEmpty(projects);

            return projects;
        }

        public async Task<Project> GetProjectByIdAsync(long id)
        {
            Project project = await _dbContext.Projects.
                FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new NotFoundException("Project not found");

            return project;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> UpdateProjectAsync(ProjectDto updateProjectDto)
        {
            Project project = await GetProjectByIdAsync(updateProjectDto.Id);

            UpdateProjectProperties(project, updateProjectDto);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task DeleteProjectAsync(long id)
        {
            Project project = await GetProjectByIdAsync(id);

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project> AssignTechnologyToProject(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await GetProjectByIdAsync(patchProjectTechnologyDto.IdProject);

            Technology technology = await GetTechnologyByIdAsync(patchProjectTechnologyDto.IdTechnology);

            await CheckIfTechnologyIsAssignedToProjectAsync(patchProjectTechnologyDto.IdProject, technology);

            project.Technologies.Add(technology);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task UnassignTechnologyFromProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await GetProjectByIdWithTechnologiesAsync(patchProjectTechnologyDto.IdProject);

            Technology technology = await GetTechnologyByIdAsync(patchProjectTechnologyDto.IdTechnology);

            CheckIfProjectContainTechnology(project, technology);

            project.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project> AssignWorkerToProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await GetProjectByIdAsync(patchProjectWorkerDto.IdProject);

            Worker worker = await GetWorkerByIdAsync(patchProjectWorkerDto.IdWorker);

            await CheckIfProjectHasMemberAsync(patchProjectWorkerDto.IdProject, worker);

            project.Members.Add(worker);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task UnassignWorkerFromProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await GetProjectWithWorkersByIdAsync(patchProjectWorkerDto.IdProject);

            Worker worker = await GetWorkerByIdAsync(patchProjectWorkerDto.IdWorker);

            CheckIfProjectContainsMember(project, worker);

            project.Members.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        private void CheckIfListIsEmpty(List<Project> projects)
        {
            if (projects.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }
        }

        private void UpdateProjectProperties(Project project, ProjectDto updateProjectDto)
        {
            project.Name = updateProjectDto.Name;
        }

        private async Task<Project> GetProjectByIdWithTechnologiesAsync(long idProject)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id.Equals(idProject))
                ?? throw new NotFoundException($"Project with id: {idProject} not found");

            return project;
        }

        private async Task<Technology> GetTechnologyByIdAsync(long idTechnology)
        {
            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(p => p.Id.Equals(idTechnology))
                ?? throw new NotFoundException($"Technology with id: {idTechnology} not found");

            return technology;
        }

        private void CheckIfProjectContainTechnology(Project project, Technology technology){
            if (!project.Technologies.Contains(technology))
            {
                throw new NotFoundException("Relation project <-> technology not found");
            }
        }

        private async Task CheckIfTechnologyIsAssignedToProjectAsync(long idProject, Technology technology)
        {
            bool existValue = await _dbContext.Projects
                .Where(p => p.Id.Equals(idProject))
                .AnyAsync(p => p.Technologies.Contains(technology));

            if (existValue)
            {
                throw new DataDuplicateException("Technology is already assigned to this project");
            }
        }

        private async Task<Project> GetProjectWithWorkersByIdAsync(long idProject)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id.Equals(idProject))
                ?? throw new NotFoundException($"Project with id: {idProject} not found");

            return project;
        }

        private async Task<Worker> GetWorkerByIdAsync(long idWorker)
        {
            Worker worker = await _dbContext.Workers
                .FirstOrDefaultAsync(w => w.Id.Equals(idWorker))
                ?? throw new NotFoundException($"Worker with id: {idWorker} not found");

            return worker;
        }

        private async Task CheckIfProjectHasMemberAsync(long idProject, Worker worker)
        {
            bool existValue = await _dbContext.Projects
                .Where(p => p.Id.Equals(idProject))
                .AnyAsync(w => w.Members.Contains(worker));

            if (existValue)
            {
                throw new DataDuplicateException("Worker is already assigned to this project!");
            }
        }

        private void CheckIfProjectContainsMember(Project project, Worker worker)
        {
            if (!project.Members.Contains(worker))
            {
                throw new NotFoundException("Relation project <-> worker not found");
            }
        }
    }
}
