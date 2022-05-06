using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Models.ProjectDto;
using WorkerManagementAPI.Services.ProjectService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            List<ProjectDto> projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById([FromRoute] long id)
        {
            ProjectDto project = await _projectService.GetProjectByIdAsync(id);
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            ProjectDto project = await _projectService.CreateProjectAsync(createProjectDto);
            return StatusCode(201, project);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDto projectDto)
        {
            ProjectDto project = await _projectService.UpdateProjectAsync(projectDto);
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] long id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}
