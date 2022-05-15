using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(long id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompanyDto);
        Task DeleteCompanyAsync(long id);
        Task<Company> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
        Task UnassignWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
    }
}
