using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.WorkerDto;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker> GetWorkerByIdAsync(long id);
        Task<Worker> CreateWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(UpdateWorkerDto updateWorkerDto);
        Task<bool> DeleteWorkerAsync(long id);
    }
}
