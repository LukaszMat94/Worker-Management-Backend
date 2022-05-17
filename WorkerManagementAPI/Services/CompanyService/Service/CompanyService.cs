using AutoMapper;
using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;
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

        public async Task<ReturnCompanyDto> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            Company createdCompany = _mapper.Map<Company>(companyDto);

            Company company = await _companyRepository.CreateCompanyAsync(createdCompany);

            ReturnCompanyDto createdCompanyDto = _mapper.Map<ReturnCompanyDto>(company);

            return createdCompanyDto;
        }

        public async Task<ReturnCompanyDto> UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto)
        {
            Company company = await _companyRepository.UpdateCompanyAsync(updateCompanyDto);

            ReturnCompanyDto companyDto = _mapper.Map<ReturnCompanyDto>(company);

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

        public async Task UnassignWorkerFromCompanyAsync(PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            await _companyRepository.UnassignWorkerFromCompanyAsync(patchCompanyWorkerDto);
        }
    }
}
