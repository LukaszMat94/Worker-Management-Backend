using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public interface IPasswordService
    {
        void HashPassword(User user);
        void VerifyPassword(User user, LoginUserDto loginUserDto);
        string GenerateJwtToken(User user);
    }
}
