using Microsoft.EntityFrameworkCore;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Models.CompanyDto;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly WorkersManagementDBContext _dbContext;

        public CompanyRepository(WorkersManagementDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            bool existValue = await _dbContext.Companies.AnyAsync(c => c.Name.Equals(company.Name));

            if (existValue)
            {
                throw new DataDuplicateException("Company with this name is already registered");
            }

            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<bool> DeleteCompanyAsync(long id)
        {
            Company company = await GetCompanyByIdAsync(id);

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            List<Company> companies = await _dbContext.Companies.ToListAsync();

            if(companies.Count == 0)
            {
                throw new NotFoundException("List is empty");
            }

            return companies;
        }

        public async Task<Company> GetCompanyByIdAsync(long id)
        {
            Company company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException("Company not found");

            return company;
        }

        public async Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompany)
        {
            bool existValue = await _dbContext.Companies.AnyAsync(c => c.Name.Equals(updatedCompany.Name) && c.Id != updatedCompany.Id);

            if (existValue)
            {
                throw new DataDuplicateException("Company already exist with registered name");
            }

            Company company = await GetCompanyByIdAsync(updatedCompany.Id);
            company.Name = updatedCompany.Name;
            await _dbContext.SaveChangesAsync();

            return company;
        }
    }
}
