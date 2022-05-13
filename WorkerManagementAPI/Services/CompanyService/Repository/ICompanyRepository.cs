﻿using WorkerManagementApi.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.CompanyDtos;

namespace WorkerManagementAPI.Services.CompanyService.Repository
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(long id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(UpdateCompanyDto updatedCompanyDto);
        Task<bool> DeleteCompanyAsync(long id);
        Task<Company> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
        Task<bool> DetachWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto);
    }
}
