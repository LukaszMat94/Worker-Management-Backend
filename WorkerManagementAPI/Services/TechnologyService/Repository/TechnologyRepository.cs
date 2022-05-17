using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.TechnologyDtos;

namespace WorkerManagementAPI.Services.TechnologyService.Repository
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public TechnologyRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Technology>> GetAllTechnologiesAsync()
        {
            return await _dbContext.Technologies.ToListAsync();
        }

        public async Task<Technology> GetTechnologyByIdAsync(long id)
        {
            return await _dbContext.Technologies
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Technology> CreateTechnologyAsync(Technology technology)
        {
            await _dbContext.Technologies.AddAsync(technology);

            return technology;
        }

        public Technology UpdateTechnology(Technology technology)
        {
            _dbContext.Entry(technology).State = EntityState.Modified;

            return technology;
        }

        public void DeleteTechnology(Technology technology)
        {
            _dbContext.Technologies.Remove(technology);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> FindIfTechnologyExistAsync(Technology technology)
        {
            bool existTechnology = await _dbContext.Technologies
                .AnyAsync(t => 
                    technology.Name == t.Name &&
                    technology.TechnologyLevel == t.TechnologyLevel);

            return existTechnology;
        }

        public async Task<bool> FindIfTechnologyExistWithOtherIdAsync(TechnologyDto technologyDto)
        {
            bool existTechnology = await _dbContext.Technologies
                .AnyAsync(t =>
                    t.Name == technologyDto.Name &&
                    t.TechnologyLevel == technologyDto.TechnologyLevel && 
                    t.Id != technologyDto.Id);

            return existTechnology;
        }

    }
}
