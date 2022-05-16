using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Data.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public CompanyRepository(WorkersManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            List<Company> companies = await _dbContext.Companies.Include(c => c.Workers).ToListAsync();

            CheckIfListIsEmpty(companies);

            return companies;
        }

        public async Task<Company> GetCompanyByIdAsync(long id)
        {
            Company company = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id.Equals(id)) 
                ?? throw new NotFoundException($"Company with id: {id} not found");

            return company;
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            await CheckIfCompanyAlreadyExistAsync(company);

            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompany)
        {
            await CheckIfCompanyAlreadyExistWithOtherIdAsync(updatedCompany);

            Company company = await GetCompanyByIdAsync(updatedCompany.Id);

            UpdateCompanyProperties(company, updatedCompany);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task DeleteCompanyAsync(long id)
        {
            Company company = await GetCompanyByIdAsync(id);

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Company> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await GetCompanyByIdAsync(patchCompanyWorkerDto.IdCompany);

            Worker worker = await GetWorkerByIdAsync(patchCompanyWorkerDto.IdWorker);

            worker.CompanyId = patchCompanyWorkerDto.IdCompany;
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task UnassignWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await GetCompanyByIdAsync(patchCompanyWorkerDto.IdCompany);

            Worker worker = await GetWorkerByIdAsync(patchCompanyWorkerDto.IdWorker);

            company.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();
        }

        private void CheckIfListIsEmpty(List<Company> companies)
        {
            if (companies.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }
        }

        private async Task CheckIfCompanyAlreadyExistAsync(Company company)
        {
            bool existValue = await _dbContext.Companies.AnyAsync(c => c.Name.Equals(company.Name));

            if (existValue)
            {
                throw new DataDuplicateException("Company with this name is already registered");
            }
        }

        private async Task CheckIfCompanyAlreadyExistWithOtherIdAsync(UpdateCompanyDto updatedCompany)
        {
            bool existValue = await _dbContext.Companies
                .AnyAsync(c => c.Name.Equals(updatedCompany.Name) && c.Id != updatedCompany.Id);

            if (existValue)
            {
                throw new DataDuplicateException("Company already exist with registered name");
            }
        }

        private void UpdateCompanyProperties(Company company, UpdateCompanyDto updatedCompany)
        {
            company.Name = updatedCompany.Name;
        }

        private async Task<Worker> GetWorkerByIdAsync(long idWorker)
        {
            Worker worker = await _dbContext.Workers
                .FirstOrDefaultAsync(w => w.Id.Equals(idWorker))
                ?? throw new NotFoundException($"Worker with id: {idWorker} not found");

            return worker;
        }
    }
}
