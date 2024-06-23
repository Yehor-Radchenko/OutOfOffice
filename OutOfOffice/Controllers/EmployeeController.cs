using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.Services;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController (EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("table-data")]
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
    public async Task<FullEmployeeViewModel> GetFullInfo(int id)
    {
        var result = await _employeeService.GetFullInfoByIdAsync(id);

        return result;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto dto)
    {
        if (ModelState.IsValid)
        {
            var employeeId = await _employeeService.AddAsync(dto);
            return Ok(employeeId);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("update/{id}")]
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
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeStatus status)
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
