using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;

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
            return await _dbContext.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(long id)
        {
            Project project = await _dbContext.Projects.
                FirstOrDefaultAsync(p => p.Id == id);

            return project;
        }

        public async Task<Project> GetProjectWithTechnologiesByIdAsync(long id)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Technologies)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project;
        }

        public async Task<Project> GetProjectWithMembersByIdAsync(long id)
        {
            Project project = await _dbContext.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);

            return project;
        }

        public void DeleteProject(Project project)
        {
            _dbContext.Projects.Remove(project);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void AssignTechnologyToProject(Project project, Technology technology)
        {
            project.Technologies.Add(technology);
        }

        public void UnassignTechnologyFromProject(Project project, Technology technology)
        {
            project.Technologies.Remove(technology);
        }

        public void AssignWorkerToProject(Project project, Worker worker)
        {
            project.Members.Add(worker);
        }

        public void UnassignWorkerFromProject(Project project, Worker worker)
        {
            project.Members.Remove(worker);
        }
    }
}
