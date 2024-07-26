using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Services;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.LeaveRequest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class LeaveRequestController : ControllerBase
{
    private readonly LeaveRequestService _leaveRequestService;

    public LeaveRequestController(LeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FullLeaveRequestViewModel>> GetLeaveRequest(int id)
    {
        try
        {
            var leaveRequest = await _leaveRequestService.GetFullInfoByIdAsync(id);
            return Ok(leaveRequest);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Employee")]
    [HttpGet("employee")]
    public async Task<ActionResult<List<EmployeeLeaveRequestViewModel>>> GetEmployeeLeaveRequests(string? searchValue)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var leaveRequests = await _leaveRequestService.GetEmployeeLeaveRequestsAsync(int.Parse(userId), searchValue);
        return Ok(leaveRequests);
    }

    [Authorize(Roles = "HRManager,ProjectManager,Admin")]
    [HttpGet("manager")]
    public async Task<ActionResult<List<TableLeaveRequestViewModel>>> GetManagerLeaveRequests(string? searchValue)
    {
        var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync(searchValue);
        return Ok(leaveRequests);
    }

    [Authorize(Roles = "Employee,HRManager,ProjectManager")]
    [HttpPost]
    public async Task<ActionResult<int>> AddLeaveRequest(LeaveRequestDto requestDto)
    {
        if (ModelState.IsValid)
        {
            int userId = GetCurrentUserIdFromToken();

            var id = await _leaveRequestService.AddAsync(userId, requestDto);
            return Ok(id);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLeaveRequest(int id, LeaveRequestDto requestDto)
    {
        var success = await _leaveRequestService.UpdateAsync(id, requestDto);
        if (success)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpPut("cancel/{id}")]
    public async Task<ActionResult> CancelLeaveRequest(int id)
    {
        var success = await _leaveRequestService.CancelRequestAsync(id);
        if (success)
            return NoContent();
        else
            return NotFound();
    }

    private int GetCurrentUserIdFromToken()
    {
        var getToken = HttpContext.Request.Cookies["jwt-token"];
        var convertToken = JwtSecurityTokenConverter.Convert(new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(getToken));
        var userId = Convert.ToInt32(convertToken.Claims.FirstOrDefault().Value);
        return userId;
    }
}
