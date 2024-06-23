using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.Services;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.BLL.ViewModels.ApprovalRequest;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.BLL.ViewModels.LeaveRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutOfOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly LeaveRequestService _leaveRequestService;

        public LeaveRequestsController(LeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

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

        [HttpGet("table")]
        public async Task<ActionResult<List<EmployeeLeaveRequestViewModel>>> GetLeaveRequestsTableData(string? searchValue)
        {
            var leaveRequests = await _leaveRequestService.GetTableDataAsync(searchValue);
            return Ok(leaveRequests);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddLeaveRequest(LeaveRequestDto requestDto)
        {
            var id = await _leaveRequestService.AddAsync(requestDto);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLeaveRequest(int id, LeaveRequestDto requestDto)
        {
            var success = await _leaveRequestService.UpdateAsync(id, requestDto);
            if (success)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelLeaveRequest(int id)
        {
            var success = await _leaveRequestService.CancelRequestAsync(id);
            if (success)
                return NoContent();
            else
                return NotFound();
        }
    }
}
