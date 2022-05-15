﻿using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(long id);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto);
        Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto);
        Task DeleteCompanyAsync(long id);
        Task<CompanyDto> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
        Task UnassignWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
    }
}
