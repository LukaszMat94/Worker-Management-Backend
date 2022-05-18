using WorkerManagementAPI.Data.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(long id);
        Task<ReturnCompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
        Task<ReturnCompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto);
        Task DeleteCompanyAsync(long id);
        Task<CompanyDto> AssignUserToCompanyAsync(PatchCompanyUserDto patchCompanyUserDto);
        Task UnassignUserFromCompanyAsync(PatchCompanyUserDto patchCompanyUserDto);
    }
}
