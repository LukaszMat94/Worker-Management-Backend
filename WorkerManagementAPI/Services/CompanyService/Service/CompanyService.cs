using AutoMapper;
using WorkerManagementApi.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Entities;
using WorkerManagementAPI.Models.CompanyDtos;
using WorkerManagementAPI.Services.CompanyService.Repository;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository, 
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            List<Company> companies = await _companyRepository.GetAllCompaniesAsync();
            List<CompanyDto> companiesDto = _mapper.Map<List<CompanyDto>>(companies);
            return companiesDto;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);
            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            Company createdCompany = _mapper.Map<Company>(companyDto);

            Company company = await _companyRepository.CreateCompanyAsync(createdCompany);

            CompanyDto createdCompanyDto = _mapper.Map<CompanyDto>(company);

            return createdCompanyDto;
        }

        public async Task<CompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto)
        {
            Company company = await _companyRepository.UpdateCompanyAsync(updateCompanyDto);
            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task DeleteCompanyAsync(long id)
        {
            await _companyRepository.DeleteCompanyAsync(id);
        }

        public async Task<CompanyDto> AssignWorkerToCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            Company company = await _companyRepository.AssignWorkerToCompanyAsync(patchCompanyWorkerDto);

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public async Task DetachWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            await _companyRepository.DetachWorkerFromCompanyAsync(patchCompanyWorkerDto);
        }
    }
}
