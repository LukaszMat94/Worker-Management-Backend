using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Data.Models.CompanyWorkerDtos;
using WorkerManagementAPI.Data.Models.CompanyDtos;
using WorkerManagementAPI.Services.CompanyService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            List<CompanyDto> companiesDto = await _companyService.GetAllCompaniesAsync();
            return Ok(companiesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] long id)
        {
            CompanyDto companyDto = await _companyService.GetCompanyByIdAsync(id);
            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto createdCompanyDto)
        {
            ReturnCompanyDto companyDto = await _companyService.CreateCompanyAsync(createdCompanyDto);
            return StatusCode(201, companyDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            ReturnCompanyDto companyDto = await _companyService.UpdateCompanyAsync(updateCompanyDto);
            return Ok(companyDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] long id)
        {
            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }

        [HttpPatch("assignWorker")]
        public async Task<IActionResult> AssignWorkerToCompany([FromBody] PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            CompanyDto companyDto = await _companyService.AssignWorkerToCompanyAsync(patchCompanyWorkerDto);
            return Ok(companyDto);
        }

        [HttpPatch("unassignWorker")]
        public async Task<IActionResult> UnassingWorkerFromCompany([FromBody] PatchCompanyWorkerDto patchCompanyWorkerDto)
        {
            await _companyService.UnassignWorkerFromCompanyAsync(patchCompanyWorkerDto);
            return NoContent();
        }
    }
}
