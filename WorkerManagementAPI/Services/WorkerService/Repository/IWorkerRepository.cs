using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.WorkerDtos;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker> GetWorkerByIdAsync(long id);
        Task<Worker> CreateWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto);
        Task<bool> DeleteWorkerAsync(long id);
        Task<Worker> AssignTechnologyToWorker(PatchWorkerTechnologyDto patchWorkerTechnologyDto);
    }
}
