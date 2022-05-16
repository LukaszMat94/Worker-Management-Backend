using AutoMapper;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.TechnologyDtos;
using WorkerManagementAPI.Exceptions;
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

        public async Task<List<TechnologyDto>> GetAllTechnologiesAsync()
        {
            List<Technology> technologies = await _technologyRepository.GetAllTechnologiesAsync();

            CheckIfListIsEmpty(technologies);

            List<TechnologyDto> technologiesDto = _mapper.Map<List<TechnologyDto>>(technologies);

            return technologiesDto;
        }

        private void CheckIfListIsEmpty(List<Technology> technologies)
        {
            if (technologies.Count == 0)
            {
                throw new NotFoundException("List technologies is empty");
            }
        }

        public async Task<TechnologyDto> GetTechnologyByIdAsync(long id)
        {
            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(id);

            CheckIfEntityIsNull(technology);

            TechnologyDto technologyDto = _mapper.Map<TechnologyDto>(technology);

            return technologyDto;
        }

        public async Task<TechnologyDto> CreateTechnologyAsync(CreateTechnologyDto createTechnologyDto)
        {
            Technology technology = _mapper.Map<Technology>(createTechnologyDto);

            await CheckIfTechnologyExistAsync(technology);

            Technology createdTechnology = await _technologyRepository.CreateTechnologyAsync(technology);

            TechnologyDto createdTechnologyDto = _mapper.Map<TechnologyDto>(createdTechnology);

            return createdTechnologyDto;
        }

        public async Task<TechnologyDto> UpdateTechnologyAsync(TechnologyDto technologyDto)
        {
            Technology technologyFromDB = await _technologyRepository.GetTechnologyByIdAsync(technologyDto.Id);

            Technology technologyToUpdate = _mapper.Map<Technology>(technologyDto);

            CheckIfEntityIsNull(technologyFromDB);

            await CheckIfAnotherTechnologyExistAsync(technologyDto);

            Technology updatedTechnology = await _technologyRepository.UpdateTechnologyAsync(technologyToUpdate);

            TechnologyDto updatedTechnologyDto = _mapper.Map<TechnologyDto>(updatedTechnology);

            return updatedTechnologyDto;
        }

        public async Task DeleteTechnologyAsync(long id)
        {
            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(id);

            CheckIfEntityIsNull(technology);

            await _technologyRepository.DeleteTechnologyAsync(id);
        }

        private void CheckIfEntityIsNull(Technology technology)
        {
            if (technology == null)
            {
                throw new NotFoundException($"Technology not found");
            }
        }

        private async Task CheckIfTechnologyExistAsync(Technology technology)
        {
            bool existValue = await _technologyRepository.FindIfTechnologyExistAsync(technology);

            if (existValue)
            {
                throw new DataDuplicateException("Technology already exist");
            }
        }

        private async Task CheckIfAnotherTechnologyExistAsync(TechnologyDto technologyDto)
        {
            bool existValue = await _technologyRepository.FindIfTechnologyExistWithOtherIdAsync(technologyDto);

            if (existValue)
            {
                throw new DataDuplicateException("Update failed, technology with name and level already registered");
            }
        }
    }
}
