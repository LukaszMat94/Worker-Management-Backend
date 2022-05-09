using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.WorkerDto;
using WorkerManagementAPI.Services.WorkerService.Repository;
using Xunit;

namespace WorkerManagementAPI.Tests.Unit.WorkerRepositoryTest
{
    public class WorkerRepositoryTest
    {
        private readonly WorkersManagementDBContext _context;
        private readonly IWorkerRepository _workerRepository;
        private List<Worker> workers = new List<Worker>();

        public WorkerRepositoryTest()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new WorkersManagementDBContext(dbOptions.Options);
            _workerRepository = new WorkerRepository(_context);
            SeedData(_context);
        }

        #region Test Get Action

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetWithValidDataTest(long id)
        {
            Worker worker = await _workerRepository.GetWorkerByIdAsync(id);
            Assert.NotNull(worker);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public async Task GetWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _workerRepository.GetWorkerByIdAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Create Action

        public static IEnumerable<object[]> CreateValidData()
        {
            yield return new object[] { new Worker { Id = 15, Name = "Jan", Surname = "Jankowski", Email = "janjankowski@gmail.com", Login = "jan", Password = "jankowski" } };
            yield return new object[] { new Worker { Id = 16, Name = "Karolina", Surname = "Karolin", Email = "karolinakarolin@gmail.com", Login = "karolina", Password = "karolin" } };
            yield return new object[] { new Worker { Id = 17, Name = "Mateusz", Surname = "Mat", Email = "mateuszmat@gmail.com", Login = "mateusz", Password = "mat" } };
        }

        [Theory]
        [MemberData(nameof(CreateValidData))]
        public async Task CreateWithValidDataTest(Worker worker)
        {
            Worker createWorker = await _workerRepository.CreateWorkerAsync(worker);
            Assert.Equal(worker, createWorker);
        }

        public static IEnumerable<object[]> CreateDuplicateData()
        {
            yield return new object[] { new Worker { Id = 15, Name = "Jan", Surname = "Jankowski", Email = "karolbrzoza@onet.pl", Login = "jan", Password = "jankowski" } };
            yield return new object[] { new Worker { Id = 16, Name = "Karolina", Surname = "Karolin", Email = "karolinakarolin@gmail.com", Login = "beata", Password = "karolin" } };
            yield return new object[] { new Worker { Id = 17, Name = "Mateusz", Surname = "Mat", Email = "rafalgwiazda@yahoo.com", Login = "mateusz", Password = "mat" } };
        }

        [Theory]
        [MemberData(nameof(CreateDuplicateData))]
        public async Task CreateWithDuplicateDataTest(Worker worker)
        {
            Func<Task> action = async () => await _workerRepository.CreateWorkerAsync(worker);
            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Update Action

        public static IEnumerable<object[]> UpdateNonExistData()
        {
            yield return new object[] { new UpdateWorkerDto { Id = 8, Name = "Tomasz", Surname = "Tomaszewski" } };
            yield return new object[] { new UpdateWorkerDto { Id = 15, Name = "Ania", Surname = "Aniołek" } };
            yield return new object[] { new UpdateWorkerDto { Id = 24, Name = "Marek", Surname = "Marecki" } };
        }

        [Theory]
        [MemberData(nameof(UpdateNonExistData))]
        public async Task UpdateWithNonExistDataTest(UpdateWorkerDto updateWorkerDto)
        {
            Func<Task> action = async () => await _workerRepository.UpdateWorkerAsync(updateWorkerDto);
            await Assert.ThrowsAsync<NotFoundException>(action);       
        }

        public static IEnumerable<object[]> UpdateValidData()
        {
            yield return new object[] { new UpdateWorkerDto { Id = 1, Name = "Tomasz", Surname = "Tomaszewski" } };
            yield return new object[] { new UpdateWorkerDto { Id = 2, Name = "Ania", Surname = "Aniołek" } };
            yield return new object[] { new UpdateWorkerDto { Id = 3, Name = "Marek", Surname = "Marecki" } };
        }

        [Theory]
        [MemberData(nameof(UpdateValidData))]
        public async Task UpdateWithValidDataTest(UpdateWorkerDto updateWorkerDto)
        {
            Worker worker = await _workerRepository.UpdateWorkerAsync(updateWorkerDto);
            Assert.Equal(new { updateWorkerDto.Id, updateWorkerDto.Name, updateWorkerDto.Surname },
                new { worker.Id, worker.Name, worker.Surname }
                );
        }

        #endregion

        #region Test Delete Action

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteWithValidDataTest(long id)
        {
            bool existValue = await _workerRepository.DeleteWorkerAsync(id);
            Assert.True(existValue);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public async Task DeleteWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _workerRepository.DeleteWorkerAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test GetList Action

        [Fact]
        public async Task GetNonExistListDataTest()
        {
            _context.Workers.RemoveRange(_context.Workers);
            _context.SaveChanges();

            Func<Task> action = async () => await _workerRepository.GetAllWorkersAsync();
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task GetExistListDataTest()
        {
            List<Worker> listWorkers = await _workerRepository.GetAllWorkersAsync();
            Assert.Equal(workers, listWorkers);
        }

        #endregion

        private void SeedData(WorkersManagementDBContext context)
        {
            workers = new()
            {
                new Worker { Id = 1, Name = "Michał", Surname = "Kowalski", Email = "michalkowalski@gmail.com", Login = "michal", Password = "kowalski" },
                new Worker { Id = 2, Name = "Joanna", Surname = "Adamczyk", Email = "joannaadamczyk@wp.pl", Login = "joanna", Password = "adamczyk" },
                new Worker { Id = 3, Name = "Karol", Surname = "Brzoza", Email = "karolbrzoza@onet.pl", Login = "karol", Password = "brzoza" },
                new Worker { Id = 4, Name = "Beata", Surname = "Nowak", Email = "beatanowak@o2.pl", Login = "beata", Password = "nowak" },
                new Worker { Id = 5, Name = "Rafał", Surname = "Gwiazda", Email = "rafalgwiazda@yahoo.com", Login = "rafal", Password = "gwiazda" },
            };
            _context.Workers.AddRange(workers);
            _context.SaveChanges();
        }
    }
}
