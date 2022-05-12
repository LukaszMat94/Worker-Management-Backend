using Microsoft.EntityFrameworkCore;
using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Context;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.WorkerDtos;

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
            List<Worker> workers = await _dbContext.Workers.ToListAsync();

            if (workers.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }

            return workers;
        }

        public async Task<Worker> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _dbContext.Workers.FirstOrDefaultAsync(w => w.Id == id) ?? throw new NotFoundException("Worker not found");

            return worker;
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            bool exist = await _dbContext.Workers.AnyAsync(w => w.Email.Equals(worker.Email));

            if (exist)
            {
                throw new DataDuplicateException("Worker already exist with given email");
            }

            await _dbContext.Workers.AddAsync(worker);

            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task<Worker> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto)
        {
            Worker worker = await GetWorkerByIdAsync(updateWorkerDto.Id);

            worker.Name = updateWorkerDto.Name;
            worker.Surname = updateWorkerDto.Surname;
            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task<bool> DeleteWorkerAsync(long id)
        {
            Worker worker = await GetWorkerByIdAsync(id);

            _dbContext.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Worker> AssignProjectToWorker(PatchWorkerProjectDto patchWorkerProjectDto)
        {
            Worker worker = await GetWorkerByIdAsync(patchWorkerProjectDto.IdWorker);

            Project project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id.Equals(patchWorkerProjectDto.IdProject))
                ?? throw new NotFoundException("Project not found");

            bool existValue = await _dbContext.Workers
                .Where(w => w.Id.Equals(patchWorkerProjectDto.IdWorker))
                .AnyAsync(w => w.Projects.Contains(project));

            if (existValue)
            {
                throw new DataDuplicateException("Project is already assigned to this worker!");
            }

            worker.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task<Worker> AssignTechnologyToWorker(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await GetWorkerByIdAsync(patchWorkerTechnologyDto.IdWorker);

            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(t => t.Id.Equals(patchWorkerTechnologyDto.IdTechnology))
                ?? throw new NotFoundException("Technology not found");

            bool existValue = await _dbContext.Workers
                .Where(w => w.Id.Equals(patchWorkerTechnologyDto.IdWorker))
                .AnyAsync(w => w.Technologies.Contains(technology));

            if (existValue)
            {
                throw new DataDuplicateException("Technology is already assigned to this worker!");
            }

            worker.Technologies.Add(technology);
            await _dbContext.SaveChangesAsync();

            return worker;
        }
    }
}
