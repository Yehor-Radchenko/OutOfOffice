using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Services;
using OutOfOffice.Common.ViewModels.Project;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ProjectManager,Admin", AuthenticationSchemes = "Bearer")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TableProjectViewModel>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FullProjectViewModel?>> GetProjectById(int id)
        {
            var project = await _projectService.GetFullProjectByIdAsync(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(ProjectDto dto)
        {
            var project = await _projectService.AddProjectAsync(dto);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProject(int id, ProjectDto dto)
        {
            try
            {
                await _projectService.UpdateProjectAsync(id, dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("deactivate/{id}")]
        public async Task<ActionResult> DeactivateProject(int id)
        {
            try
            {
                await _projectService.DeactivateProjectAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{projectId}/employees/{employeeId}")]
        public async Task<ActionResult> AddEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                var success = await _projectService.AddEmployeeToProjectAsync(projectId, employeeId);
                if (!success)
                    return BadRequest("Employee is already part of the project.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{projectId}/employees/{employeeId}")]
        public async Task<ActionResult> RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}
