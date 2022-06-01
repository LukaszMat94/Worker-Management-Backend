using AutoMapper;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Services.CompanyService.Repository;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.UserService.Repository;
using WorkerManagementAPI.ExceptionsTemplate;

namespace WorkerManagementAPI.Services.CompanyService.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
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
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_COMPANY_LIST_NOTFOUND);
            }
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);

            CheckIfCompanyEntityIsNull(company);

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        private void CheckIfCompanyEntityIsNull(Company company)
        {
            if(company == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_COMPANY_NOTFOUND);
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
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_COMPANY_CONFLICT);
            }
        }

        public async Task<ReturnCompanyDto> UpdateCompanyAsync(UpdateCompanyDto companyDto)
        {
            Company companyFromDB = await _companyRepository.GetCompanyByIdAsync(companyDto.Id);

            CheckIfCompanyEntityIsNull(companyFromDB);

            await CheckIfAnotherCompanyExistAsync(companyDto);

            UpdateCompanyProperties(companyFromDB, companyDto);

            await _companyRepository.SaveChangesAsync();

            ReturnCompanyDto updatedCompanyDto = _mapper.Map<ReturnCompanyDto>(companyFromDB);

            return updatedCompanyDto;
        }

        private async Task CheckIfAnotherCompanyExistAsync(UpdateCompanyDto updateCompanyDto)
        {
            bool existValue = await _companyRepository.FindIfAnotherCompanyExistAsync(updateCompanyDto);

            if (existValue)
            {
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_COMPANY_CONFLICT);
            }
        }

        private void UpdateCompanyProperties(Company company, UpdateCompanyDto companyDto)
        {
            company.Name = companyDto.Name;
        }

        public async Task DeleteCompanyAsync(long id)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(id);

            CheckIfCompanyEntityIsNull(company);

            _companyRepository.DeleteCompany(company);

            await _companyRepository.SaveChangesAsync();
        }

        public async Task<CompanyDto> AssignUserToCompanyAsync(PatchCompanyUserDto patchCompanyUserDto)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(patchCompanyUserDto.IdCompany);

            CheckIfCompanyEntityIsNull(company);

            User user = await _userRepository.GetUserByIdAsync(patchCompanyUserDto.IdUser);

            CheckIfUserEntityIsNull(user);

            CheckIfRelationExist(company, user);

            _companyRepository.AssignUserToCompany(company, user);

            await _companyRepository.SaveChangesAsync();

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        private void CheckIfUserEntityIsNull(User user)
        {
            if(user == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_USER_NOTFOUND);
            }
        }

        private void CheckIfRelationExist(Company company, User user)
        {
            if (user.CompanyId.Equals(company.Id))
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_RELATION_CONFLICT);
            }
        }

        public async Task UnassignUserFromCompanyAsync(PatchCompanyUserDto patchCompanyUserDto)
        {
            Company company = await _companyRepository.GetCompanyByIdAsync(patchCompanyUserDto.IdCompany);

            CheckIfCompanyEntityIsNull(company);

            User user = await _userRepository.GetUserByIdAsync(patchCompanyUserDto.IdUser);

            CheckIfUserEntityIsNull(user);

            CheckIfRelationNonExist(company, user);

            _companyRepository.UnassignUserFromCompany(company, user);

            await _companyRepository.SaveChangesAsync();
        }

        private void CheckIfRelationNonExist(Company company, User user)
        {
            if (!user.CompanyId.Equals(company.Id))
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_RELATION_NOTFOUND);
            }
        }
    }
}
