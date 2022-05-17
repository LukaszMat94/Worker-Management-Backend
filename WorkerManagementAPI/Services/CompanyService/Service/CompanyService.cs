﻿using AutoMapper;
using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Services.CompanyService.Repository;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.WorkerService.Repository;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository,
            IWorkerRepository workerRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _workerRepository = workerRepository;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            List<Company> companies = await _companyRepository.GetAllCompaniesAsync();

            CheckIfListIsEmpty(companies);

            List<CompanyDto> companiesDto = _mapper.Map<List<CompanyDto>>(companies);

            return companiesDto;
        }

        private void CheckIfListIsEmpty(List<Company> companies)
        {
            if(companies == null)
            {
                throw new NotFoundException("List is empty");
            }
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);

            CheckIfEntityIsNull(company);

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        private void CheckIfEntityIsNull(Object entity)
        {
            if(entity == null)
            {
                throw new NotFoundException($"{nameof(entity)} not found");
            }
        }

        public async Task<ReturnCompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            Company createCompany = _mapper.Map<Company>(companyDto);

            await CheckIfCompanyExistAsync(createCompany);

            Company addedCompany = await _companyRepository.CreateCompanyAsync(createCompany);

            await _companyRepository.SaveChangesAsync();

            ReturnCompanyDto addedCompanyDto = _mapper.Map<ReturnCompanyDto>(addedCompany);

            return addedCompanyDto;
        }

        private async Task CheckIfCompanyExistAsync(Company company)
        {
            bool existValue = await _companyRepository.FindIfCompanyExistAsync(company);

            if (existValue)
            {
                throw new DataDuplicateException("Company already exist");
            }
        }

        public async Task<ReturnCompanyDto> UpdateCompanyAsync(UpdateCompanyDto companyDto)
        {
            Company companyFromDB = await _companyRepository.GetCompanyByIdAsync(companyDto.Id);

            CheckIfEntityIsNull(companyFromDB);

            await CheckIfAnotherCompanyExistAsync(companyDto);

            Company companyToUpdate = _mapper.Map<Company>(companyDto);

            Company company = _companyRepository.UpdateCompany(companyToUpdate);

            await _companyRepository.SaveChangesAsync();

            ReturnCompanyDto updatedCompanyDto = _mapper.Map<ReturnCompanyDto>(company);

            return updatedCompanyDto;
        }

        private async Task CheckIfAnotherCompanyExistAsync(UpdateCompanyDto updateCompanyDto)
        {
            bool existValue = await _companyRepository.FindIfAnotherCompanyExistAsync(updateCompanyDto);

            if (existValue)
            {
                throw new DataDuplicateException("Update failed, Company already exist");
            }
        }

        public async Task DeleteCompanyAsync(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);

            CheckIfEntityIsNull(company);

            _companyRepository.DeleteCompany(company);

            await _companyRepository.SaveChangesAsync();
        }

        public async Task<CompanyDto> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(patchCompanyWorkerDto.IdCompany);

            CheckIfEntityIsNull(company);

            Worker worker = await _workerRepository.GetWorkerByIdAsync(patchCompanyWorkerDto.IdWorker);

            CheckIfEntityIsNull(worker);

            CheckIfRelationExist(company, worker);

            _companyRepository.AssignWorkerToCompany(company, worker);

            await _companyRepository.SaveChangesAsync();

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        private void CheckIfRelationExist(Company company, Worker worker)
        {
            if (worker.CompanyId.Equals(company.Id))
            {
                throw new NotFoundException("Worker is assigned to this company");
            }
        }

        public async Task UnassignWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(patchCompanyWorkerDto.IdCompany);

            CheckIfEntityIsNull(company);

            Worker worker = await _workerRepository.GetWorkerByIdAsync(patchCompanyWorkerDto.IdWorker);

            CheckIfEntityIsNull(worker);

            CheckIfRelationNonExist(company, worker);

            _companyRepository.UnassignWorkerFromCompany(company, worker);

            await _companyRepository.SaveChangesAsync();
        }

        private void CheckIfRelationNonExist(Company company, Worker worker)
        {
            if (!worker.CompanyId.Equals(company.Id))
            {
                throw new NotFoundException("Relation not found");
            }
        }
    }
}
