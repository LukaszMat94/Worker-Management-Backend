using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.UserService.Repository
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> LoginUserAsync(User user);
        Task SaveChangesAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserWithTechnologiesByIdAsync(long id);
        void DeleteUser(User user);
        Task<bool> FindIfUserExistAsync(User user);
        void AssignTechnologyToUser(User user, Technology technology);
        void UnassignTechnologyFromUser(User user, Technology technology);
        void AssignProjectToUser(User user, Project project);
        void UnassignProjectFromUser(User user, Project project);
    }
}
