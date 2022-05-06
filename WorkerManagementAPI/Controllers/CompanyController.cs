﻿using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Models.CompanyDto;
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
            CompanyDto companyDto = await _companyService.CreateCompanyAsync(createdCompanyDto);
            return StatusCode(201, companyDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            CompanyDto companyDto = await _companyService.UpdateCompanyAsync(updateCompanyDto);
            return Ok(companyDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] long id)
        {
            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }
    }
}