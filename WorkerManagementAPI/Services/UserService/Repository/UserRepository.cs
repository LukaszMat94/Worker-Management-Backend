using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;

namespace WorkerManagementAPI.Services.UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public UserRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            return new User();
        }

        public async Task<User> LoginUserAsync(User user)
        {
            return new User();
        }

        public async Task SaveChangesAsync()
        {

        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            User user = await _dbContext.Users
                .FirstOrDefaultAsync(w => w.Id == id);

            return user;
        }

        public async Task<User> GetUserWithTechnologiesByIdAsync(long id)
        {
            User user = await _dbContext.Users
                .Include(w => w.Technologies)
                .FirstOrDefaultAsync(w => w.Id == id);

            return user;
        }

        public void DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<bool> FindIfUserExistAsync(User user)
        {
            bool existValue = await _dbContext.Users
                .AnyAsync(w => w.Email.Equals(user.Email));

            return existValue;
        }

        public void AssignTechnologyToUser(User user, Technology technology)
        {
            user.Technologies.Add(technology);
        }

        public void UnassignTechnologyFromUser(User user, Technology technology)
        {
            user.Technologies.Remove(technology);
        }

        public void AssignProjectToUser(User user, Project project)
        {
            user.Projects.Add(project);
        }

        public void UnassignProjectFromUser(User user, Project project)
        {
            user.Projects.Remove(project);
        }
    }
}
