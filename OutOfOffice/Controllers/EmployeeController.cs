﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Models;
using System.Security.Claims;
using OutOfOffice.BLL.Services;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService, UserManager<Employee> userManager)
    {
        _employeeService = employeeService;
    }

    [HttpGet("table-data")]
    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    public async Task<IActionResult> GetTableViewModels(string? search)
    {
        List<TableEmployeeViewModel> query;

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

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<FullEmployeeViewModel>> GetMyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await _employeeService.GetFullInfoByIdAsync(int.Parse(userId));
        return Ok(result);
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

    [HttpGet("photo")]
    [Authorize]
    public async Task<IActionResult> GetPhotoAsync([FromQuery] int employeeId)
    {
        try
        {
            var photo = await _employeeService.GetPhotoByIdAsync(employeeId);
            return Ok(photo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("photo/remove")]
    [Authorize]
    public async Task<IActionResult> RemovePhotoAsync([FromQuery] int employeeId)
    {
        try
        {
            await _employeeService.RemovePhotoAsync(employeeId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("photo/upload")]
    [Authorize]
    public async Task<IActionResult> UploadPhotoToEmployeeAsync([FromQuery] int employeeId, [FromBody] string photoBase64)
    {
        try
        {
            await _employeeService.UploadPhotoToEmployeeAsync(employeeId, photoBase64);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
