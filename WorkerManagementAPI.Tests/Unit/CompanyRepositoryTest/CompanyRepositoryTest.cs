using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Services.CompanyService.Repository;
using Xunit;

namespace WorkerManagementAPI.Tests.Unit.CompanyRepositoryTest
{
    public class CompanyRepositoryTest
    {
        private readonly WorkersManagementDBContext _context;
        private readonly ICompanyRepository _companyRepository;
        private List<Company> companies = new List<Company>();
        private List<Worker> workers = new List<Worker>();

        public CompanyRepositoryTest()
        {
            DbContextOptionsBuilder optionsDbBuiler = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _context = new WorkersManagementDBContext(optionsDbBuiler.Options);
            _companyRepository = new CompanyRepository(_context);
            SeedCompaniesData(_context);
            SeedWorkersData(_context);
        }

        #region Test Get Action

        [Theory]

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetWithValidDataTest(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);
            Assert.NotNull(company);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public async Task GetWithNonExistDataTest(long id)
        {
            Func<Task> action = async () => await _companyRepository.GetCompanyByIdAsync(id);
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Create Action

        public static IEnumerable<object[]> CreateValidData()
        {
            yield return new object[] { new Company { Id = 15, Name = "Hochland" } };
            yield return new object[] { new Company { Id = 16, Name = "Mullermilch" } };
            yield return new object[] { new Company { Id = 17, Name = "Mlekovita" } };
        }

        [Theory]
        [MemberData(nameof(CreateValidData))]
        public async Task CreateWithValidDataTest(Company company)
        {
            Company createdCompany = await _companyRepository.CreateCompanyAsync(company);
            Assert.Equal(company.Name, createdCompany.Name);
        }

        public static IEnumerable<object[]> CreateDuplicateData()
        {
            yield return new object[] { new Company { Id = 25, Name = "Samsung" } };
            yield return new object[] { new Company { Id = 26, Name = "Tesla" } };
            yield return new object[] { new Company { Id = 27, Name = "T-Mobile" } };
        }

        [Theory]
        [MemberData(nameof(CreateDuplicateData))]
        public async Task CreateWithDuplicateDataTest(Company company)
        {
            Func<Task> action = async () => await _companyRepository.CreateCompanyAsync(company);
            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Update Action

        public static IEnumerable<object[]> UpdateValidData()
        {
            yield return new object[] { new UpdateCompanyDto { Id = 1, Name = "Mazda" } };
            yield return new object[] { new UpdateCompanyDto { Id = 2, Name = "Maybach" } };
            yield return new object[] { new UpdateCompanyDto { Id = 4, Name = "Rexona" } };
        }

        [Theory]
        [MemberData(nameof(UpdateValidData))]
        public async Task UpdateWithValidDataTest(UpdateCompanyDto updateCompanyDto)
        {
            Company company = await _companyRepository.UpdateCompanyAsync(updateCompanyDto);
            Assert.Equal(updateCompanyDto.Name, company.Name);
        }

        public static IEnumerable<object[]> UpdateDuplicateData()
        {
            yield return new object[] { new UpdateCompanyDto { Id = 1, Name = "Samsung" } };
            yield return new object[] { new UpdateCompanyDto { Id = 2, Name = "T-Mobile" } };
            yield return new object[] { new UpdateCompanyDto { Id = 5, Name = "Tesla" } };
        }

        [Theory]
        [MemberData(nameof(UpdateDuplicateData))]
        public async Task UpdateWithDuplicateDataTest(UpdateCompanyDto updateCompanyDto)
        {
            Func<Task> action = async () => await _companyRepository.UpdateCompanyAsync(updateCompanyDto);
            await Assert.ThrowsAsync<DataDuplicateException>(action);
        }

        #endregion

        #region Test Delete Action

        #endregion

        #region Test GetList Action

        [Fact]
        public async Task GetExistListDataTest()
        {
            List<Company> listCompanies = await _companyRepository.GetAllCompaniesAsync();
            Assert.Equal(companies, listCompanies);
        }

        [Fact]
        public async Task GetEmptyListDataTest()
        {
            _context.Companies.RemoveRange(_context.Companies);
            _context.SaveChanges();

            Func<Task> action = async () => await _companyRepository.GetAllCompaniesAsync();
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        #endregion

        #region Test Patch Worker To Company Action

        public static IEnumerable<object[]> PatchWorkerToCompanyData()
        {
            yield return new object[] { new PatchCompanyWorkerDto { IdCompany = 1, IdWorker = 5 } };
            yield return new object[] { new PatchCompanyWorkerDto { IdCompany = 2, IdWorker = 2 } };
            yield return new object[] { new PatchCompanyWorkerDto { IdCompany = 5, IdWorker = 1 } };
        }

        [Theory]
        [MemberData(nameof(PatchWorkerToCompanyData))]
        public async Task PatchWorkerToCompanyValidTest(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await _companyRepository.AssignWorkerToCompanyAsync(patchCompanyWorkerDto);
            Assert.Equal(workers.Find(w => w.Id == patchCompanyWorkerDto.IdWorker).Name, company.Workers.Find(w => w.Id == patchCompanyWorkerDto.IdWorker).Name);
        }

        #endregion

        private void SeedCompaniesData(WorkersManagementDBContext context)
        {
            companies = new()
            {
                new Company { Id = 1, Name = "Tesla" },
                new Company { Id = 2, Name = "Danone" },
                new Company { Id = 3, Name = "Mercedes" },
                new Company { Id = 4, Name = "T-Mobile" },
                new Company { Id = 5, Name = "Samsung" },
            };

            _context.Companies.AddRange(companies);
            _context.SaveChanges();
        }

        private void SeedWorkersData(WorkersManagementDBContext context)
        {
            workers = new()
            {
                new Worker { Id = 1, Name = "Michal", Surname = "Wojcik", Company = null},
                new Worker { Id = 2, Name = "Marian", Surname = "Kanapa", Company = null },
                new Worker { Id = 3, Name = "Mieczyslaw", Surname = "Kolos", Company = null },
                new Worker { Id = 4, Name = "Katarzyna", Surname = "Mieczyk", Company = null },
                new Worker { Id = 5, Name = "Iwona", Surname = "Mikolajczyk", Company = null }
            };

            _context.Workers.AddRange(workers);
            _context.SaveChanges();
        }

    }
}
