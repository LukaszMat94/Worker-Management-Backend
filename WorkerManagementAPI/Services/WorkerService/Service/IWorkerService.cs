using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Models.WorkerDtos;

namespace WorkerManagementAPI.Services.WorkerService.Service
{
    public interface IWorkerService
    {
        Task<List<WorkerDto>> GetAllWorkersAsync();
        Task<WorkerDto> GetWorkerByIdAsync(long id);
        Task<WorkerDto> CreateWorkerAsync(CreateWorkerDto worker);
        Task<WorkerDto> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto);
        Task DeleteWorkerAsync(long id);
        Task<UpdateWorkerTechnologyDto> AssignTechnologyToWorker(PatchWorkerTechnologyDto patchWorkerTechnologyDto);
    }
}
