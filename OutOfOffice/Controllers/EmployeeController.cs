using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Services;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;
    private readonly UserManager<Employee> _userManager;

    public EmployeeController (EmployeeService employeeService, UserManager<Employee> userManager)
    {
        _employeeService = employeeService;
        _userManager = userManager;
    }

    [HttpGet("table-data")]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<IActionResult> GetTableViewModels(string? search)
    {
        var query = new List<TableEmployeeViewModel>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string searchLower = search.ToLower();
            query = await _employeeService.GetTableDataAsync(searchLower);
        }
        else
        {
            query = await _employeeService.GetTableDataAsync();
        }
        return Ok(query);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<FullEmployeeViewModel> GetFullInfo(int id)
    {
        var result = await _employeeService.GetFullInfoByIdAsync(id);

        return result;
    }

    [HttpGet]
    [Authorize]
    public async Task<FullEmployeeViewModel> GetFullInfoAboutAuthentificatedEmployee()
    {
        var result = await _employeeService.GetFullInfoByIdAsync(int.Parse(_userManager.GetUserId(User)));

        return result;
    }

    [HttpPost]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto dto)
    {
        if (ModelState.IsValid)
        {
            var employeeId = await _employeeService.RegisterAsync(dto);
            return Ok(employeeId);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromForm] EmployeeDto dto)
    {
        if (ModelState.IsValid)
        {
            var employeeId = await _employeeService.UpdateAsync(id, dto);
            return Ok(employeeId);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("changeStatus/{id}")]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<IActionResult> ChangeStatusAsync(int id, [FromBody] EmployeeStatus status)
    {
        if (ModelState.IsValid)
        {
            var employeeId = await _employeeService.ChangeStatusAsync(id, status);
            return Ok(employeeId);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}
