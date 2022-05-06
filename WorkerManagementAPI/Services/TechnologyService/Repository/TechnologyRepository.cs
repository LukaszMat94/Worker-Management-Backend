using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.TechnologyDto;

namespace WorkerManagementAPI.Services.TechnologyService.Repository
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public TechnologyRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Technology> CreateTechnologyAsync(Technology technology)
        {
            await _dbContext.Technologies.AddAsync(technology);
            await _dbContext.SaveChangesAsync();
            return technology;
        }

        public async Task DeleteTechnologyAsync(long id)
        {
            Technology technology = await GetTechnologyByIdAsync(id);

            _dbContext.Technologies.Remove(technology);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Technology>> GetAllTechnologiesAsync()
        {
            List<Technology> technologies = await _dbContext.Technologies.ToListAsync();

            return technologies;
        }

        public async Task<Technology> GetTechnologyByIdAsync(long id)
        {
            Technology technology = await _dbContext.Technologies.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Technology not found");

            return technology;
        }

        public async Task<Technology> UpdateTechnologyAsync(TechnologyDto technologyDto)
        {
            Technology technology = await GetTechnologyByIdAsync(technologyDto.Id);

            technology.Name = technologyDto.Name;
            technology.TechnologyLevel = technologyDto.TechnologyLevel;
            await _dbContext.SaveChangesAsync();

            return technology;
        }

    }
}
