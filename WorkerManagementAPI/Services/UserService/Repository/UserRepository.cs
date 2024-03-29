﻿using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;

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
            await _dbContext.Users.AddAsync(user);
            return user;
        }

        public async Task<User> LoginUserAsync(User user)
        {
            return new User();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
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

        public async Task<User> GetUserWithRoleByEmailAsync(string email)
        {
            User user = await _dbContext.Users
                .Include(w => w.Role)
                .FirstOrDefaultAsync(w => w.Email.Equals(email));

            return user;
        }

        public async Task<AccountStatusEnum> GetUserAccountStatus(User user)
        {
            AccountStatusEnum accountStatus = await _dbContext.Users.Where(u => u.Id.Equals(user.Id))
                .Select(u => u.AccountStatus)
                .SingleOrDefaultAsync();

            return accountStatus;
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
