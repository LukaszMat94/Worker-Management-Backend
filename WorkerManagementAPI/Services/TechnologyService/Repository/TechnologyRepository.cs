using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;
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
            List<Technology> technologies = await _dbContext.Technologies.ToListAsync();

            CheckIfListIsEmpty(technologies);

            return technologies;
        }

        public async Task<Technology> GetTechnologyByIdAsync(long id)
        {
            Technology technology = await _dbContext.Technologies
                .FirstOrDefaultAsync(x => x.Id == id) 
                ?? throw new NotFoundException("Technology not found");

            return technology;
        }

        public async Task<Technology> CreateTechnologyAsync(Technology technology)
        {
            await CheckIfTechnologyExistAsync(technology);

            await _dbContext.Technologies.AddAsync(technology);
            await _dbContext.SaveChangesAsync();
            return technology;

        }

        public async Task<Technology> UpdateTechnologyAsync(TechnologyDto technologyDto)
        {
            await CheckIfTechnologyExistWithOtherIdAsync(technologyDto);

            Technology technology = await GetTechnologyByIdAsync(technologyDto.Id);

            UpdateTechnology(technology, technologyDto);
            await _dbContext.SaveChangesAsync();

            return technology;
        }

        public async Task DeleteTechnologyAsync(long id)
        {
            Technology technology = await GetTechnologyByIdAsync(id);

            _dbContext.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

        private void CheckIfListIsEmpty(List<Technology> technologies)
        {
            if (technologies.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }
        }

        private async Task CheckIfTechnologyExistAsync(Technology technology)
        {
            bool existTechnology = await _dbContext.Technologies
               .AnyAsync(c => technology.Name == c.Name && 
               technology.TechnologyLevel == c.TechnologyLevel);

            if (existTechnology)
            {
                throw new DataDuplicateException("Technology already exist");
            }
        }

        private async Task CheckIfTechnologyExistWithOtherIdAsync(TechnologyDto technologyDto)
        {
            bool existAnotherTechnology = await _dbContext.Technologies.AnyAsync(c =>
                c.Name == technologyDto.Name && 
                c.TechnologyLevel == technologyDto.TechnologyLevel && 
                c.Id != technologyDto.Id);

            if (existAnotherTechnology)
            {
                throw new DataDuplicateException("You cannot update this technology because another one exist just in db!");
            }
        }

        private void UpdateTechnology(Technology technology, TechnologyDto technologyDto)
        {
            technology.Name = technologyDto.Name;
            technology.TechnologyLevel = technologyDto.TechnologyLevel;
        }
    }
}
