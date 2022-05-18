using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Data.Context;
using WorkerManagementAPI.Data.Entities;
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
            return await _dbContext.Companies
                .Include(c => c.Users).ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(long id)
        {
            Company company = await _dbContext.Companies
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            return company;
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            await _dbContext.Companies.AddAsync(company);

            return company;
        }

        public void DeleteCompany(Company company)
        {
            _dbContext.Companies.Remove(company);
        }

        public void AssignUserToCompany(Company company, User user)
        {
            user.Company = company;
        }

        public void UnassignUserFromCompany(Company company, User user)
        {
            company.Users.Remove(user);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> FindIfCompanyExistAsync(Company company)
        {
            bool existValue = await _dbContext.Companies
                .AnyAsync(c => c.Name.Equals(company.Name));

            return existValue;
        }

        public async Task<bool> FindIfAnotherCompanyExistAsync(UpdateCompanyDto updateCompanyDto)
        {
            bool existValue = await _dbContext.Companies
                .AnyAsync(c => c.Name.Equals(updateCompanyDto.Name) &&
                    c.Id != updateCompanyDto.Id);

            return existValue;
        }
    }
}
