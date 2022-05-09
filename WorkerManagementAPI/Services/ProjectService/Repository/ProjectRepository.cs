using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.ProjectDto;

namespace WorkerManagementAPI.Services.ProjectService.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public ProjectRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteProjectAsync(long id)
        {
            Project project = await GetProjectByIdAsync(id);

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
            return true;
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

        public async Task<Project> UpdateProjectAsync(ProjectDto updateProjectDto)
        {
            Project project = await GetProjectByIdAsync(updateProjectDto.Id);

            project.Name = updateProjectDto.Name;
            await _dbContext.SaveChangesAsync();

            return project;
        }
    }
}
