using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;

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

            CheckIfListIsEmpty(workers);

            return workers;
        }

        public async Task<Worker> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _dbContext.Workers.FirstOrDefaultAsync(w => w.Id == id) 
                ?? throw new NotFoundException("Worker not found");

            return worker;
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            await CheckIfEmailIsTakenAsync(worker);

            await _dbContext.Workers.AddAsync(worker);
            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task<Worker> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto)
        {
            Worker worker = await GetWorkerByIdAsync(updateWorkerDto.Id);

            UpdateWorkerProperties(worker, updateWorkerDto);
            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task DeleteWorkerAsync(long id)
        {
            Worker worker = await GetWorkerByIdAsync(id);

            _dbContext.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Worker> AssignTechnologyToWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await GetWorkerByIdAsync(patchWorkerTechnologyDto.IdWorker);

            Technology technology = await GetTechnologyAsync(patchWorkerTechnologyDto.IdTechnology);

            await CheckIfTechnologyIsAssignedAsync(patchWorkerTechnologyDto.IdWorker, technology);

            worker.Technologies.Add(technology);
            await _dbContext.SaveChangesAsync();

            return worker;
        }

        public async Task UnassignTechnologyFromWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await GetWorkerWithTechnologiesAsync(patchWorkerTechnologyDto.IdWorker);

            Technology technology = await GetTechnologyAsync(patchWorkerTechnologyDto.IdTechnology);

            CheckIfExistRelation(worker, technology);

            worker.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

        private void CheckIfListIsEmpty(List<Worker> workers)
        {
            if (workers.Count == 0)
                {
                    throw new NotFoundException("List is empty");
                }
        }

        private async Task CheckIfEmailIsTakenAsync(Worker worker)
        {
            bool exist = await _dbContext.Workers.AnyAsync(w => w.Email.Equals(worker.Email));

            if (exist)
            {
                throw new DataDuplicateException("Worker already exist with given email");
            }
        }

        private void UpdateWorkerProperties(Worker worker, UpdateWorkerDto updateWorkerDto)
        {
            worker.Name = updateWorkerDto.Name;
            worker.Surname = updateWorkerDto.Surname;
        }

        private async Task<Worker> GetWorkerWithTechnologiesAsync(long idWorker)
        {
            Worker worker = await _dbContext.Workers
                .Include(w => w.Technologies)
                .FirstOrDefaultAsync(w => w.Id == idWorker) 
                ?? throw new NotFoundException("Worker not found");

            return worker;
        }

        private async Task<Technology> GetTechnologyAsync(long idTechnology)
        {
            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(t => t.Id.Equals(idTechnology)) 
                ?? throw new NotFoundException("Technology not found");

            return technology;
        }

        private void CheckIfExistRelation(Worker worker, Technology technology)
        {
            bool existWorkerWithTechnology = !worker.Technologies.Contains(technology);

            if (!existWorkerWithTechnology)
            {
                throw new NotFoundException("Relation not found");
            }
        }

        private async Task CheckIfTechnologyIsAssignedAsync(long idWorker, Technology technology)
        {
            bool existValue = await _dbContext.Workers
                .Where(w => w.Id.Equals(idWorker))
                .AnyAsync(w => w.Technologies.Contains(technology));

            if (existValue)
            {
                throw new DataDuplicateException("Technology is already assigned to this worker!");
            }
        }
    }
}
