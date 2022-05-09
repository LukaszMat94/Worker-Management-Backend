using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.CompanyDto;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(long id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompanyDto);
        Task<bool> DeleteCompanyAsync(long id);
    }
}
