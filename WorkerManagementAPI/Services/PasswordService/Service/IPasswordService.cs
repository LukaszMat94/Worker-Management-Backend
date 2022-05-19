using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public interface IPasswordService
    {
        void HashPassword(User user);
    }
}
