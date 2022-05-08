using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.TechnologyDto;

namespace WorkerManagementAPI.Services.TechnologyService.Repository
{
    public interface ITechnologyRepository
    {
        Task<List<Technology>> GetAllTechnologiesAsync();
        Task<Technology> GetTechnologyByIdAsync(long id);
        Task<Technology> CreateTechnologyAsync(Technology technology);
        Task<Technology> UpdateTechnologyAsync(TechnologyDto technologyDto);
        Task<bool> DeleteTechnologyAsync(long id);
    }
}
