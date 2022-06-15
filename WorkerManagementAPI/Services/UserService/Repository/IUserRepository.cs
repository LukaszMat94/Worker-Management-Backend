using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;

namespace WorkerManagementAPI.Services.UserService.Repository
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task SaveChangesAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserWithTechnologiesByIdAsync(long id);
        Task<User> GetUserWithRoleByEmailAsync(string email);
        Task<User> GetUserWithRoleById(long id);
        Task<AccountStatusEnum> GetUserAccountStatus(User user);
        void DeleteUser(User user);
        Task<bool> FindIfUserExistAsync(User user);
        void AssignTechnologyToUser(User user, Technology technology);
        void UnassignTechnologyFromUser(User user, Technology technology);
        void AssignProjectToUser(User user, Project project);
        void UnassignProjectFromUser(User user, Project project);
    }
}
