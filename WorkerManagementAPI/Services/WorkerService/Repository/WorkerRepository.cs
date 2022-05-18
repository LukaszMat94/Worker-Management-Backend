using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly WorkersManagementDBContext _dbContext;
        public WorkerRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _dbContext.Workers.ToListAsync();
        }

        public async Task<Worker> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _dbContext.Workers
                .FirstOrDefaultAsync(w => w.Id == id);

            return worker;
        }

        public async Task<Worker> GetWorkerWithTechnologiesByIdAsync(long id)
        {
            Worker worker = await _dbContext.Workers
                .Include(w => w.Technologies)
                .FirstOrDefaultAsync(w => w.Id == id);

            return worker;
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            await _dbContext.Workers.AddAsync(worker);

            return worker;
        }

        public void DeleteWorker(Worker worker)
        {
            _dbContext.Workers.Remove(worker);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> FindIfWorkerExistAsync(Worker worker)
        {
            bool existValue = await _dbContext.Workers
                .AnyAsync(w => w.Email.Equals(worker.Email));

            return existValue;
        }

        public void AssignTechnologyToWorker(Worker worker, Technology technology)
        {
            worker.Technologies.Add(technology);
        }

        public void UnassignTechnologyFromWorker(Worker worker, Technology technology)
        {
            worker.Technologies.Remove(technology);
        }

        public void AssignProjectToWorker(Worker worker, Project project)
        {
            worker.Projects.Add(project);
        }

        public void UnassignProjectFromWorker(Worker worker, Project project)
        {
            worker.Projects.Remove(project);
        }
    }
}
