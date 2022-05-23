using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.UserService.Repository;
using Xunit;

namespace WorkerManagementAPI.Tests.Unit.UserRepositoryTest
{
    public class UserRepositoryTest
    {
        private readonly WorkersManagementDBContext _context;
        private readonly IUserRepository _UserRepository;
        private List<User> Users = new List<User>();

        public UserRepositoryTest()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new WorkersManagementDBContext(dbOptions.Options);
            _UserRepository = new UserRepository(_context);
            SeedUserData(_context);
        }

        #region Test Get Action

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetWithValidDataTest(long id)
        {
            User User = await _UserRepository.GetUserByIdAsync(id);
            Assert.NotNull(User);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public async Task GetWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _UserRepository.GetUserByIdAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Create Action

        public static IEnumerable<object[]> CreateValidData()
        {
            yield return new object[] { new User { Id = 15, Name = "Jan", Surname = "Jankowski", Email = "janjankowski@gmail.com", Password = "jankowski" } };
            yield return new object[] { new User { Id = 16, Name = "Karolina", Surname = "Karolin", Email = "karolinakarolin@gmail.com", Password = "karolin" } };
            yield return new object[] { new User { Id = 17, Name = "Mateusz", Surname = "Mat", Email = "mateuszmat@gmail.com", Password = "mat" } };
        }

        [Theory]
        [MemberData(nameof(CreateValidData))]
        public async Task CreateWithValidDataTest(User User)
        {
            User createUser = await _UserRepository.RegisterUserAsync(User);
            Assert.Equal(User, createUser);
        }

        public static IEnumerable<object[]> CreateDuplicateData()
        {
            yield return new object[] { new User { Id = 15, Name = "Jan", Surname = "Jankowski", Email = "karolbrzoza@onet.pl", Password = "jankowski" } };
            yield return new object[] { new User { Id = 16, Name = "Karolina", Surname = "Karolin", Email = "joannaadamczyk@wp.pl", Password = "karolin" } };
            yield return new object[] { new User { Id = 17, Name = "Mateusz", Surname = "Mat", Email = "michalkowalski@gmail.com", Password = "mat" } };
        }

        [Theory]
        [MemberData(nameof(CreateDuplicateData))]
        public async Task CreateWithDuplicateDataTest(User User)
        {
            Func<Task> action = async () => await _UserRepository.RegisterUserAsync(User);
            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Delete Action

        #endregion

        #region Test GetList Action

        [Fact]
        public async Task GetNonExistListDataTest()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            Func<Task> action = async () => await _UserRepository.GetAllUsersAsync();
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task GetExistListDataTest()
        {
            List<User> listUsers = await _UserRepository.GetAllUsersAsync();
            Assert.Equal(Users, listUsers);
        }

        #endregion

        private void SeedUserData(WorkersManagementDBContext context)
        {
            Users = new()
            {
                new User { Id = 1, Name = "Michał", Surname = "Kowalski", Email = "michalkowalski@gmail.com", Password = "kowalski" },
                new User { Id = 2, Name = "Joanna", Surname = "Adamczyk", Email = "joannaadamczyk@wp.pl", Password = "adamczyk" },
                new User { Id = 3, Name = "Karol", Surname = "Brzoza", Email = "karolbrzoza@onet.pl", Password = "brzoza" }
            };

            _context.Users.AddRange(Users);
            _context.SaveChanges();
        }
    }
}
