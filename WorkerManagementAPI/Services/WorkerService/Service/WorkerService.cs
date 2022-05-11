using AutoMapper;
using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.WorkerDtos;
using WorkerManagementAPI.Services.WorkerService.Repository;

namespace WorkerManagementAPI.Services.WorkerService.Service
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IMapper _mapper;

        public WorkerService(IWorkerRepository workerRepository,
            IMapper mapper)
        {
            _workerRepository = workerRepository;
            _mapper = mapper;
        }

        public async Task<UpdateWorkerTechnologyDto> AssignTechnologyToWorker(PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            Worker worker = await _workerRepository.AssignTechnologyToWorker(patchWorkerTechnologyDto);

            UpdateWorkerTechnologyDto updateWorkerTechnologyDto = _mapper.Map<UpdateWorkerTechnologyDto>(worker);

            return updateWorkerTechnologyDto;
        }

        public async Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto createWorkerDto)
        {
            Worker createWorker = _mapper.Map<Worker>(createWorkerDto);

            Worker worker = await _workerRepository.CreateWorkerAsync(createWorker);

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }

        public async Task DeleteWorkerAsync(long id)
        {
            await _workerRepository.DeleteWorkerAsync(id);
        }

        public async Task<List<WorkerDto>> GetAllWorkersAsync()
        {
            List<Worker> workers = await _workerRepository.GetAllWorkersAsync();

            List<WorkerDto> workersDto = _mapper.Map<List<WorkerDto>>(workers);

            return workersDto;
        }

        public async Task<WorkerDto> GetWorkerByIdAsync(long id)
        {
            Worker worker = await _workerRepository.GetWorkerByIdAsync(id);

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }

        public async Task<WorkerDto> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto)
        {
            Worker worker = await _workerRepository.UpdateWorkerAsync(updateWorkerDto);

            WorkerDto workerDto = _mapper.Map<WorkerDto>(worker);

            return workerDto;
        }
    }
}
