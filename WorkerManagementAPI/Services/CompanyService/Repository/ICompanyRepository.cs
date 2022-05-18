using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(long id);
        Task<Company> CreateCompanyAsync(Company company);
        void DeleteCompany(Company company);
        Task SaveChangesAsync();
        Task<bool> FindIfCompanyExistAsync(Company company);
        Task<bool> FindIfAnotherCompanyExistAsync(UpdateCompanyDto updateCompanyDto);
        void AssignWorkerToCompany(Company company, Worker worker);
        void UnassignWorkerFromCompany(Company company, Worker worker);
    }
}
