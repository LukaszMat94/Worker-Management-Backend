using WorkerManagementAPI.Models.TechnologyDtos;

namespace WorkerManagementAPI.Services.TechnologyService.Service
{
    public interface ITechnologyService
    {
        Task<List<TechnologyDto>> GetAllTechnologiesAsync();
        Task<TechnologyDto> GetTechnologyByIdAsync(long id);
        Task<TechnologyDto> CreateTechnologyAsync(CreateTechnologyDto createTechnologyDto);
        Task<TechnologyDto> UpdateTechnologyAsync(TechnologyDto technologyDto);
        Task DeleteTechnologyAsync(long id);
    }
}
