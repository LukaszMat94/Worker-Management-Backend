﻿using Microsoft.AspNetCore.Mvc;
using WorkerManagementApi.Data.Models.WorkerDtos;
using WorkerManagementAPI.Models.WorkerDtos;
using WorkerManagementAPI.Services.WorkerService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/workers")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkers()
        {
            List<WorkerDto> workersDto = await _workerService.GetAllWorkersAsync();
            return Ok(workersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById([FromRoute] long id)
        {
            WorkerDto workerDto = await _workerService.GetWorkerByIdAsync(id);
            return Ok(workerDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto createWorkerDto)
        {
            WorkerDto workerDto = await _workerService.CreateWorkerAsync(createWorkerDto);
            return StatusCode(201, workerDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorker([FromBody] UpdateWorkerDto updateWorkerDto)
        {
            WorkerDto workerDto = await _workerService.UpdateWorkerAsync(updateWorkerDto);
            return Ok(workerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker([FromRoute] long id)
        {
            await _workerService.DeleteWorkerAsync(id);
            return NoContent();
        }

        [HttpPatch("assignTechnology")]
        public async Task<IActionResult> AssignTechnologyToWorker([FromBody] PatchWorkerTechnologyDto patchWorkerTechnologyDto)
        {
            UpdateWorkerTechnologyDto updateWorkerTechnologyDto = await _workerService.AssignTechnologyToWorker(patchWorkerTechnologyDto);
            return Ok(updateWorkerTechnologyDto);
        }

        [HttpPatch("assignProject")]
        public async Task<IActionResult> AssignProjectToWorker([FromBody] PatchWorkerProjectDto patchWorkerProjectDto)
        {
            UpdateWorkerProjectDto updateWorkerProjectDto = await _workerService.AssignProjectToWorker(patchWorkerProjectDto);
            return Ok(updateWorkerProjectDto);
        }
    }
}
