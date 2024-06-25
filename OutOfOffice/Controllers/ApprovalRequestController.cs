using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Common.Dto.ApprovalRequestDto;
using OutOfOffice.Common.Services;
using OutOfOffice.DAL.Models;
using System.IdentityModel.Tokens.Jwt;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HRManager,ProjectManager,Admin")]
public class ApprovalRequestController : ControllerBase
{
    private readonly ApprovalRequestService _approvalRequestService;

    public ApprovalRequestController(ApprovalRequestService approvalRequestService)
    {
        _approvalRequestService = approvalRequestService;
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
            int approverId = GetCurrentUserIdFromToken();

            var result = await _approvalRequestService.HandleApprovalRequestAsync(dto, approverId);
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
            int approverId = GetCurrentUserIdFromToken();

            var result = await _approvalRequestService.UpdateApprovalRequestAsync(id, dto, approverId);
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

    private int GetCurrentUserIdFromToken()
    {
        var getToken = HttpContext.Request.Cookies["jwt-token"];
        var convertToken = JwtSecurityTokenConverter.Convert(new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(getToken));
        var approverId = Convert.ToInt32(convertToken.Claims.FirstOrDefault().Value);
        return approverId;
    }
}
