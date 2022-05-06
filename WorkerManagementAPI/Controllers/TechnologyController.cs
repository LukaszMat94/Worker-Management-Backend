using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Models.TechnologyDto;
using WorkerManagementAPI.Services.TechnologyService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/technologies")]
    [ApiController]
    public class TechnologyController : ControllerBase
    {
        private readonly ITechnologyService _technologyService;

        public TechnologyController(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTechnologies()
        {
            List<TechnologyDto> technologies = await _technologyService.GetAllTechnologiesAsync();
            return Ok(technologies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTechnologyById([FromRoute] long id)
        {
            TechnologyDto technology = await _technologyService.GetTechnologyByIdAsync(id);
            return Ok(technology);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTechnology([FromBody] CreateTechnologyDto createTechnologyDto)
        {
            TechnologyDto technology = await _technologyService.CreateTechnologyAsync(createTechnologyDto);
            return StatusCode(201, technology);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTechnology([FromBody] TechnologyDto technologyDto)
        {
            TechnologyDto technology = await _technologyService.UpdateTechnologyAsync(technologyDto);
            return Ok(technology);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnology([FromRoute] long id)
        {
            await _technologyService.DeleteTechnologyAsync(id);
            return NoContent();
        }
    }
}
