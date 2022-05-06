using AutoMapper;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.TechnologyDto;
using WorkerManagementAPI.Services.TechnologyService.Repository;

namespace WorkerManagementAPI.Services.TechnologyService.Service
{
    public class TechnologyService : ITechnologyService
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IMapper _mapper;

        public TechnologyService(ITechnologyRepository technologyRepository,
            IMapper mapper)
        {
            _technologyRepository = technologyRepository;
            _mapper = mapper;
        }

        public async Task<TechnologyDto> CreateTechnologyAsync(CreateTechnologyDto createTechnologyDto)
        {
            Technology technology = _mapper.Map<Technology> (createTechnologyDto);

            Technology createdTechnology = await _technologyRepository.CreateTechnologyAsync(technology);

            TechnologyDto createdTechnologyDto = _mapper.Map<TechnologyDto>(createdTechnology);

            return createdTechnologyDto;
        }

        public async Task DeleteTechnologyAsync(long id)
        {
            await _technologyRepository.DeleteTechnologyAsync(id);
        }

        public async Task<List<TechnologyDto>> GetAllTechnologiesAsync()
        {
            List<Technology> technologies = await _technologyRepository.GetAllTechnologiesAsync();

            List<TechnologyDto> technologiesDto = _mapper.Map<List<TechnologyDto>>(technologies);

            return technologiesDto;
        }

        public async Task<TechnologyDto> GetTechnologyByIdAsync(long id)
        {
            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(id);

            TechnologyDto technologyDto = _mapper.Map<TechnologyDto>(technology);

            return technologyDto;
        }

        public async Task<TechnologyDto> UpdateTechnologyAsync(TechnologyDto technologyDto)
        {
            Technology technology = await _technologyRepository.UpdateTechnologyAsync(technologyDto);

            TechnologyDto updatedTechnologyDto = _mapper.Map<TechnologyDto>(technology);

            return updatedTechnologyDto;
        }
    }
}
