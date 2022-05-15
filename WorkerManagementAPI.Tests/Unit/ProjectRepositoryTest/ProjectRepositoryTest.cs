using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Services.ProjectService.Repository;
using Xunit;

namespace WorkerManagementAPI.Tests.Unit.ProjectRepositoryTest
{
    public class ProjectRepositoryTest
    {
        private readonly IProjectRepository _projectRepository;
        private readonly WorkersManagementDBContext _context;
        private List<Project> projects = new List<Project>();

        public ProjectRepositoryTest()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new WorkersManagementDBContext(dbOptions.Options);
            _projectRepository = new ProjectRepository(_context);
            SeedProjectData(_context);
        }

        #region Test Get Action

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetWithValidDataTest(long id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);
            Assert.NotNull(project);
        }

        [Theory]
        [InlineData(98)]
        [InlineData(99)]
        [InlineData(100)]
        public async Task GetWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _projectRepository.GetProjectByIdAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Create Action

        public static IEnumerable<object[]> CreateValidData()
        {
            yield return new object[] { new Project { Id = 18, Name = "Kurs Go" } };
            yield return new object[] { new Project { Id = 19, Name = "Kurs HTML" } };
            yield return new object[] { new Project { Id = 20, Name = "Kurs Laravel" } };
        }

        [Theory]
        [MemberData(nameof(CreateValidData))]
        public async Task CreateDuplicateDataTest(Project project)
        {
            Project createdProject = await _projectRepository.CreateProjectAsync(project);
            Assert.Equal(project.Name, createdProject.Name);
        }

        #endregion

        #region Test Update Action

        public static IEnumerable<object[]> UpdateValidData()
        {
            yield return new object[] { new ProjectDto { Id = 8, Name = "Kurs Zaawansowany C++"} };
            yield return new object[] { new ProjectDto { Id = 9, Name = "Kurs Początkujący C++" } };
            yield return new object[] { new ProjectDto { Id = 10, Name = "Kurs Zaawansowany Java" } };
        }

        [Theory]
        [MemberData(nameof(UpdateValidData))]
        public async Task UpdateWithValidDataTest(ProjectDto projectDto)
        {
            Project project = await _projectRepository.UpdateProjectAsync(projectDto);
            Assert.Equal(
                new {project.Id,  project.Name}, new { projectDto.Id, projectDto.Name}
                );
        }

        #endregion

        #region Test Delete Action

        #endregion

        #region Test GetList Action
        [Fact]
        public async Task GetExistListDataTest()
        {
            List<Project> listProjects = await _projectRepository.GetAllProjectsAsync();
            Assert.Equal(projects, listProjects);
        }

        [Fact]
        public async Task GetEmptyListDataTest()
        {
            _context.Projects.RemoveRange(_context.Projects);
            _context.SaveChanges();

            Func<Task> action = async () => await _projectRepository.GetAllProjectsAsync();
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        private void SeedProjectData(WorkersManagementDBContext context)
        {
            projects = new()
            {
                new Project { Id = 1, Name = "Kurs C#" },
                new Project { Id = 2, Name = "Kurs Java" },
                new Project { Id = 3, Name = "Kurs PHP" },
                new Project { Id = 4, Name = "Kurs Ruby" },
                new Project { Id = 5, Name = "Kurs C++" },
                new Project { Id = 6, Name = "Kurs Javascript" },
                new Project { Id = 7, Name = "Kurs Html" },
                new Project { Id = 8, Name = "Kurs CSS" },
                new Project { Id = 9, Name = "Kurs React" },
                new Project { Id = 10, Name = "Kurs Angular" }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
}
