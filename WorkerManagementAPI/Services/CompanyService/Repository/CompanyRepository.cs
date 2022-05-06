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
            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task DeleteCompanyAsync(long id)
        {
            Company company = await GetCompanyByIdAsync(id);

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            List<Company> companies = await _dbContext.Companies.ToListAsync();

            return companies;
        }

        public async Task<Company> GetCompanyByIdAsync(long id)
        {
            Company company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException("Company not found");

            return company;
        }


        public async Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompany)
        {
            Company company = await GetCompanyByIdAsync(updatedCompany.Id);
            company.Name = updatedCompany.Name;
            await _dbContext.SaveChangesAsync();

            return company;
        }
    }
}
