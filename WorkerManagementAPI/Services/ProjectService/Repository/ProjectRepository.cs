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

            if(projects.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }

            return projects;
        }

        public async Task<Project> GetProjectByIdAsync(long id)
        {
            Project project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException("Project not found");

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

            project.Name = updateProjectDto.Name;
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

            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(p => p.Id.Equals(patchProjectTechnologyDto.IdTechnology))
                ?? throw new NotFoundException("Technology not found");

            bool existValue = await _dbContext.Projects
                .Where(p => p.Id.Equals(patchProjectTechnologyDto.IdProject))
                .AnyAsync(p => p.Technologies.Contains(technology));

            if (existValue)
            {
                throw new DataDuplicateException("Technology is already assigned to this project");
            }

            project.Technologies.Add(technology);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task UnassignTechnologyFromProjectAsync(PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id == patchProjectTechnologyDto.IdProject)
                ?? throw new NotFoundException("Project not found");

            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(t => t.Id.Equals(patchProjectTechnologyDto.IdTechnology))
                ?? throw new NotFoundException("Technology not found");

            if (!project.Technologies.Contains(technology))
            {
                throw new NotFoundException("Relation not found");
            }

            project.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project> AssignWorkerToProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await GetProjectByIdAsync(patchProjectWorkerDto.IdProject);

            Worker worker = await _dbContext.Workers
                .FirstOrDefaultAsync(w => w.Id.Equals(patchProjectWorkerDto.IdWorker))
                ?? throw new NotFoundException("Worker not found");

            bool existValue = await _dbContext.Projects
                .Where(p => p.Id.Equals(patchProjectWorkerDto.IdProject))
                .AnyAsync(w => w.Members.Contains(worker));

            if (existValue)
            {
                throw new DataDuplicateException("Worker is already assigned to this project!");
            }

            project.Members.Add(worker);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task UnassignWorkerFromProjectAsync(PatchProjectWorkerDto patchProjectWorkerDto)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == patchProjectWorkerDto.IdProject)
                ?? throw new NotFoundException("Project not found");


            Worker worker = await _dbContext.Workers
                .FirstOrDefaultAsync(w => w.Id.Equals(patchProjectWorkerDto.IdWorker))
                ?? throw new NotFoundException("Worker not found");

            if (!project.Members.Contains(worker))
            {
                throw new NotFoundException("Relation not found");
            }

            project.Members.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }
    }
}
