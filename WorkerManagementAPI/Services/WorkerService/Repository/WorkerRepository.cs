﻿using Microsoft.EntityFrameworkCore;
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

            if (workers.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }

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

        public async Task DeleteWorkerAsync(long id)
        {
            Worker worker = await GetWorkerByIdAsync(id);

            _dbContext.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Worker> AssignTechnologyToWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
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

        public async Task UnassignTechnologyFromWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await _dbContext.Workers
                .Include(w => w.Technologies)
                .FirstOrDefaultAsync(w => w.Id == patchWorkerTechnologyDto.IdWorker)
                ?? throw new NotFoundException("Worker not found");

            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(t => t.Id.Equals(patchWorkerTechnologyDto.IdTechnology))
                ?? throw new NotFoundException("Technology not found");

            bool existWorkerWithTechnology = !worker.Technologies.Contains(technology);

            if (!existWorkerWithTechnology)
            {
                throw new NotFoundException("Relation not found");
            }

            worker.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

    }
}
