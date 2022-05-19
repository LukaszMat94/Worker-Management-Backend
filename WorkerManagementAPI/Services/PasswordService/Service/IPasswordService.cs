using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public interface IPasswordService
    {
        void HashPassword(User user, String password);
        void VerifyPassword(User user, LoginUserDto loginUserDto);
        string GenerateTemporaryPassword();
        string GenerateJwtToken(User user);
    }
}
