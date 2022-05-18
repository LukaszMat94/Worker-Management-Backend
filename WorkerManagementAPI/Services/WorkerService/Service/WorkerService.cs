using AutoMapper;
using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Services.WorkerService.Repository;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Exceptions;

namespace WorkerManagementAPI.Services.WorkerService.Service
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IMapper _mapper;

        public WorkerService(IWorkerRepository workerRepository,
            ITechnologyRepository technologyRepository,
            IMapper mapper)
        {
            _workerRepository = workerRepository;
            _technologyRepository = technologyRepository;
            _mapper = mapper;
        }

        public async Task<List<WorkerDto>> GetAllWorkersAsync()
        {
            List<Worker> workers = await _workerRepository.GetAllWorkersAsync();

            CheckIfListIsNull(workers);

            List<WorkerDto> workersDto = _mapper.Map<List<WorkerDto>>(workers);

            return workersDto;
        }

        private void CheckIfListIsNull(List<Worker> workers)
        {
            if(workers == null)
            {
                throw new NotFoundException("List is empty");
            }
        }

        public async Task<WorkerDto> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _workerRepository.GetWorkerByIdAsync(id);

            CheckIfWorkerEntityIsNull(worker);

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }

        private void CheckIfWorkerEntityIsNull(Worker worker)
        {
            if (worker == null)
            {
                throw new NotFoundException("Worker not found");
            }
        }

        public async Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto createWorkerDto)
        {
            Worker createWorker = _mapper.Map<Worker>(createWorkerDto);

            await CheckIfWorkerExistAsync(createWorker);

            Worker worker = await _workerRepository.CreateWorkerAsync(createWorker);

            await _workerRepository.SaveChangesAsync();

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }

        private async Task CheckIfWorkerExistAsync(Worker worker)
        {
            bool existValue = await _workerRepository.FindIfWorkerExistAsync(worker);

            if (existValue)
            {
                throw new DataDuplicateException("Worker already exist");
            }
        }

        public async Task<WorkerDto> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto)
        {
            Worker worker = await _workerRepository.GetWorkerByIdAsync(updateWorkerDto.Id);

            CheckIfWorkerEntityIsNull(worker);

            UpdateWorkerProperties(worker, updateWorkerDto);

            await _workerRepository.SaveChangesAsync();

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }

        private void UpdateWorkerProperties(Worker worker, UpdateWorkerDto updateWorkerDto)
        {
            worker.Name = updateWorkerDto.Name;
            worker.Surname = updateWorkerDto.Surname;
        }

        public async Task DeleteWorkerAsync(long id)
        {
            Worker worker = await _workerRepository.GetWorkerByIdAsync(id);

            CheckIfWorkerEntityIsNull(worker);

            _workerRepository.DeleteWorker(worker);

            _workerRepository.SaveChangesAsync();
        }

        public async Task<UpdateWorkerTechnologyDto> AssignTechnologyToWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await _workerRepository.GetWorkerWithTechnologiesByIdAsync(patchWorkerTechnologyDto.IdWorker);

            CheckIfWorkerEntityIsNull(worker);

            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchWorkerTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationWorkerTechnologyExist(worker, technology);

            _workerRepository.AssignTechnologyToWorker(worker, technology);

            await _workerRepository.SaveChangesAsync();

            UpdateWorkerTechnologyDto updateWorkerTechnologyDto = _mapper.Map<UpdateWorkerTechnologyDto>(worker);

            return updateWorkerTechnologyDto;
        }

        private void CheckIfTechnologyEntityIsNull(Technology technology)
        {
            if(technology == null)
            {
                throw new NotFoundException("Technology not found");
            }
        }

        private void CheckIfRelationWorkerTechnologyExist(Worker worker, Technology technology)
        {
            if (worker.Technologies.Contains(technology))
            {
                throw new DataDuplicateException("Relation already exist");
            }
        }

        public async Task UnassignTechnologyFromWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await _workerRepository.GetWorkerWithTechnologiesByIdAsync(patchWorkerTechnologyDto.IdWorker);

            CheckIfWorkerEntityIsNull(worker);

           Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchWorkerTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationWorkerTechnologyNonExist(worker, technology);

            _workerRepository.UnassignTechnologyFromWorker(worker, technology);

            await _workerRepository.SaveChangesAsync();
        }

        private void CheckIfRelationWorkerTechnologyNonExist(Worker worker, Technology technology)
        {
            if (!worker.Technologies.Contains(technology))
            {
                throw new NotFoundException("Relation not exist");
            }
        }
    }
}
