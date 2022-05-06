using WorkerManagementAPI.Models.CompanyDto;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(long id);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
        Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto);
        Task DeleteCompanyAsync(long id);
    }
}
