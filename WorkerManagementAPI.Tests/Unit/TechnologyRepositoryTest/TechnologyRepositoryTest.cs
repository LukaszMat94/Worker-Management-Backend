using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Data.Models.TechnologyDtos;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using Xunit;

namespace WorkerManagementAPI.Tests.Unit.TechnologyRepositoryTest
{
    public class TechnologyRepositoryTest
    {
        private readonly WorkersManagementDBContext _context;
        private readonly ITechnologyRepository _technologyRepository;
        private List<Technology> technologies = new List<Technology>();

        public TechnologyRepositoryTest()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new WorkersManagementDBContext(dbOptions.Options);
            _technologyRepository = new TechnologyRepository(_context);

            SeedData(_context);
        }

        #region Test Get Action

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetWithValidDataTest(long id)
        {
            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(id);
            Assert.NotNull(technology);
        }

        [Theory]
        [InlineData(997)]
        [InlineData(998)]
        [InlineData(999)]
        public async Task GetWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _technologyRepository.GetTechnologyByIdAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Create Action

        public static IEnumerable<object[]> CreateValidData()
        {
            yield return new object[] { new Technology { Id = 777, Name = "Visual Basic", TechnologyLevel = TechnologyLevelEnum.Basic } };
            yield return new object[] { new Technology { Id = 778, Name = "Thymeleaf", TechnologyLevel = TechnologyLevelEnum.Advanced } };
            yield return new object[] { new Technology { Id = 779, Name = "Go", TechnologyLevel = TechnologyLevelEnum.Basic } };
        }

        [Theory]
        [MemberData(nameof(CreateValidData))]
        public async Task CreateWithValidDataTest(Technology technology)
        {
            Technology createdTechnology = await _technologyRepository.CreateTechnologyAsync(technology);

            Assert.Equal(new { technology.Name, technology.TechnologyLevel }, 
                new { createdTechnology.Name, createdTechnology.TechnologyLevel}
                );
        }

        public static IEnumerable<object[]> CreateDuplicateData()
        {
            yield return new object[] { new Technology { Id = 248, Name = "Java", TechnologyLevel = TechnologyLevelEnum.Basic } };
            yield return new object[] { new Technology { Id = 249, Name = "C#", TechnologyLevel = TechnologyLevelEnum.Advanced } };
            yield return new object[] { new Technology { Id = 250, Name = "Ruby", TechnologyLevel = TechnologyLevelEnum.Basic } };
        }

        [Theory]
        [MemberData(nameof(CreateDuplicateData))]
        public async Task CreateWithDuplicateDataTest(Technology technology)
        {
            Func<Task> action = async () => await _technologyRepository.CreateTechnologyAsync(technology);

            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Update Action

        public static IEnumerable<object[]> UpdateValidData()
        {
            yield return new object[] { new TechnologyDto { Id = 1, Name = "Angielski", TechnologyLevel = TechnologyLevelEnum.Basic } };
            yield return new object[] { new TechnologyDto { Id = 2, Name = "Niemiecki", TechnologyLevel = TechnologyLevelEnum.Medium } };
            yield return new object[] { new TechnologyDto { Id = 3, Name = "Rosyjski", TechnologyLevel = TechnologyLevelEnum.Medium } };
        }

        [Theory]
        [MemberData(nameof(UpdateValidData))]
        public async Task UpdateWithValidDataTest(TechnologyDto technologyDto)
        {
            Technology updatedTechnology = await _technologyRepository.UpdateTechnologyAsync(technologyDto);

            Assert.Equal(technologyDto.Name, updatedTechnology.Name);
        }

        public static IEnumerable<object[]> UpdateDuplicateData()
        {
            yield return new object[] { new TechnologyDto { Id = 8, Name = "Python", TechnologyLevel = TechnologyLevelEnum.Medium} };
            yield return new object[] { new TechnologyDto { Id = 9, Name = "Javascript", TechnologyLevel = TechnologyLevelEnum.None } };
            yield return new object[] { new TechnologyDto { Id = 10, Name = "React", TechnologyLevel = TechnologyLevelEnum.Basic } };
        }

        [Theory]
        [MemberData(nameof(UpdateDuplicateData))]
        public async Task UpdateWithDuplicateDataTest(TechnologyDto technologyDto)
        {
            Func<Task> action = async () => await _technologyRepository.UpdateTechnologyAsync(technologyDto);

            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Delete Action

        #endregion

        #region Test GetList Action

        [Fact]
        public async Task GetExistListDataTest()
        {
            List<Technology> listTechnologies = await _technologyRepository.GetAllTechnologiesAsync();
            Assert.Equal(technologies, listTechnologies);
        }

        [Fact]
        public async Task GetEmptyListDataTest()
        {
            _context.Technologies.RemoveRange(_context.Technologies);
            _context.SaveChanges();

            Func<Task> action = async () => await _technologyRepository.GetAllTechnologiesAsync();
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        private void SeedData(WorkersManagementDBContext context)
        {
            technologies = new()
            {
                new Technology { Id = 1, Name = "Java", TechnologyLevel = TechnologyLevelEnum.Basic },
                new Technology { Id = 2, Name = "C#", TechnologyLevel = TechnologyLevelEnum.Advanced },
                new Technology { Id = 3, Name = "Ruby", TechnologyLevel = TechnologyLevelEnum.Basic },
                new Technology { Id = 4, Name = "C++", TechnologyLevel = TechnologyLevelEnum.Medium },
                new Technology { Id = 5, Name = "Python", TechnologyLevel = TechnologyLevelEnum.Medium },
                new Technology { Id = 6, Name = "Javascript", TechnologyLevel = TechnologyLevelEnum.None },
                new Technology { Id = 7, Name = "React", TechnologyLevel = TechnologyLevelEnum.Basic },
                new Technology { Id = 8, Name = "HTML", TechnologyLevel = TechnologyLevelEnum.Advanced },
                new Technology { Id = 9, Name = "CSS", TechnologyLevel = TechnologyLevelEnum.Medium },
                new Technology { Id = 10, Name = "PHP", TechnologyLevel = TechnologyLevelEnum.Advanced }
            };
            context.Technologies.AddRange(technologies);
            context.SaveChanges();
        }
    }
}