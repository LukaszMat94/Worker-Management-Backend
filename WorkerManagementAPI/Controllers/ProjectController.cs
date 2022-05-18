﻿using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Data.Models.ProjectDtos;
using WorkerManagementAPI.Data.Models.WorkerDtos;
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

        [HttpPatch("assignTechnology")]
        public async Task<IActionResult> AssignTechnologyToProject([FromBody] PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            UpdateProjectTechnologyDto updateProjectTechnologyDto = await _projectService.AssignTechnologyToProjectAsync(patchProjectTechnologyDto);
            return Ok(updateProjectTechnologyDto);
        }

        [HttpPatch("unassignTechnology")]
        public async Task<IActionResult> UnassignTechnologyFromProject([FromBody] PatchProjectTechnologyDto patchProjectTechnologyDto)
        {
            await _projectService.UnassignTechnologyFromProjectAsync(patchProjectTechnologyDto);
            return NoContent();
        }

        [HttpPatch("assignWorker")]
        public async Task<IActionResult> AssignWorkerToProject([FromBody] PatchProjectWorkerDto patchProjectWorkerDto)
        {
            UpdateProjectWorkerDto updateProjectWorkerDto = await _projectService.AssignWorkerToProjectAsync(patchProjectWorkerDto);
            return Ok(updateProjectWorkerDto);
        }

        [HttpPatch("unassignWorker")]
        public async Task<IActionResult> UnassignWorkerFromProject([FromBody] PatchProjectWorkerDto patchProjectWorkerDto)
        {
            await _projectService.UnassignWorkerFromProjectAsync(patchProjectWorkerDto);
            return NoContent();
        }
    }
}
