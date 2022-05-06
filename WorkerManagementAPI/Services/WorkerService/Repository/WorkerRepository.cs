using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.WorkerDto;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly WorkersManagementDBContext _dbContext;
        public WorkerRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            await _dbContext.Workers.AddAsync(worker);

            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task DeleteWorkerAsync(long id)
        {
            Worker worker = await GetWorkerByIdAsync(id);

            _dbContext.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            List<Worker> workers = await _dbContext.Workers.ToListAsync();

            return workers;
        }

        public async Task<Worker> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _dbContext.Workers.FirstOrDefaultAsync(w => w.Id == id) ?? throw new NotFoundException("Worker not found");

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
    }
}
