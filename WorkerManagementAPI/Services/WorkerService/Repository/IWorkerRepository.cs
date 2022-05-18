using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.WorkerService.Repository
{
    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker> GetWorkerByIdAsync(long id);
        Task<Worker> GetWorkerWithTechnologiesByIdAsync(long id);
        Task<Worker> CreateWorkerAsync(Worker worker);
        void DeleteWorker(Worker worker);
        Task SaveChangesAsync();
        Task<bool> FindIfWorkerExistAsync(Worker worker);
        void AssignTechnologyToWorker(Worker worker, Technology technology);
        void UnassignTechnologyFromWorker(Worker worker, Technology technology);
        void AssignProjectToWorker(Worker worker, Project project);
        void UnassignProjectFromWorker(Worker worker, Project project);
    }
}
