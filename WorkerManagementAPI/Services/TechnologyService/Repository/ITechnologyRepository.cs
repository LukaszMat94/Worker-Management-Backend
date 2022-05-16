using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.TechnologyDtos;

namespace WorkerManagementAPI.Services.TechnologyService.Repository
{
    public interface ITechnologyRepository
    {
        Task<List<Technology>> GetAllTechnologiesAsync();
        Task<Technology> GetTechnologyByIdAsync(long id);
        Task<Technology> CreateTechnologyAsync(Technology technology);
        Task<Technology> UpdateTechnologyAsync(Technology technology);
        Task DeleteTechnologyAsync(long id);
        Task<bool> FindIfTechnologyExistAsync(Technology technology);
        Task<bool> FindIfTechnologyExistWithOtherIdAsync(TechnologyDto technologyDto);
    }
}
