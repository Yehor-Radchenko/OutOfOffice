using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Dto.ApprovalRequestDto;
using OutOfOffice.BLL.Services;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HRManager,ProjectManager,Admin")]
public class ApprovalRequestController : ControllerBase
{
    private readonly ApprovalRequestService _approvalRequestService;
    private readonly UserManager<Employee> _userManager;

    public ApprovalRequestController(ApprovalRequestService approvalRequestService, UserManager<Employee> userManager)
    {
        _approvalRequestService = approvalRequestService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApprovalRequests(string? searchValue)
    {
        var approvalRequests = await _approvalRequestService.GetAllApprovalRequestsAsync(searchValue);
        return Ok(approvalRequests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetApprovalRequestInfo(int id)
    {
        try
        {
            var approvalRequest = await _approvalRequestService.GetFullInfoByIdAsync(id);
            return Ok(approvalRequest);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("handle")]
    public async Task<IActionResult> HandleApprovalRequest([FromBody] CreateApprovalRequestDto dto)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var approverUserId = currentUser.Id;

            var result = await _approvalRequestService.HandleApprovalRequestAsync(dto, approverUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApprovalRequest(int id, [FromBody] UpdateApprovalRequestDto dto)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var approverUserId = currentUser.Id;

            var result = await _approvalRequestService.UpdateApprovalRequestAsync(id, dto, approverUserId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
