using WorkerManagementAPI.Data.Models.WorkerDtos;
using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker> GetWorkerByIdAsync(long id);
        Task<Worker> CreateWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto);
        Task DeleteWorkerAsync(long id);
        Task<Worker> AssignTechnologyToWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto);
        Task UnassignTechnologyFromWorkerAsync(PatchWorkerTechnologyDto patchWorkerTechnologyDto);
    }
}
