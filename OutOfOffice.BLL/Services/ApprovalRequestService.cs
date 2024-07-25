using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Dto.ApprovalRequestDto;
using OutOfOffice.Common.ViewModels.ApprovalRequest;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.BLL.Services;

public class ApprovalRequestService
{
    private readonly OutOfOfficeDbContext _context;

    public ApprovalRequestService(OutOfOfficeDbContext context)
    {
        _context = context;
    }

    public async Task<FullApprovalRequestViewModel?> GetFullInfoByIdAsync(int id)
    {
        var approvalRequest = await _context.ApprovalRequests
            .Include(ar => ar.Approver)
            .Include(ar => ar.LeaveRequest)
            .FirstOrDefaultAsync(ar => ar.Id == id);

        if (approvalRequest == null)
        {
            throw new KeyNotFoundException($"ApprovalRequest with ID {id} not found.");
        }

        var viewModel = new FullApprovalRequestViewModel
        {
            Id = approvalRequest.Id,
            ApproverId = approvalRequest.ApproverId,
            LeaveRequestId = approvalRequest.LeaveRequestId,
            Status = approvalRequest.Status.ToString(),
            Comment = approvalRequest.Comment,
            Approver = new BriefEmployeeViewModel
            {
                Id = approvalRequest.Approver.Id,
                FullName = approvalRequest.Approver.FullName,
            }
        };

        return viewModel;
    }

    public async Task<List<TableApprovalRequestViewModel>> GetAllApprovalRequestsAsync(string? searchValue = null)
    {
        var query = _context.ApprovalRequests
            .Include(ar => ar.LeaveRequest)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
        {
            searchValue = searchValue.ToLower();
            query = query.Where(ar =>
                ar.Id.ToString().Contains(searchValue));
        }

        var approvalRequests = await query.ToListAsync();

        var viewModelList = approvalRequests.Select(ar => new TableApprovalRequestViewModel
        {
            Id = ar.Id,
            ApproverId = ar.ApproverId,
            LeaveRequestId = ar.LeaveRequestId,
            Status = ar.Status.ToString(),
            Comment = ar.Comment
        }).ToList();

        return viewModelList;
    }

    public async Task<bool> HandleApprovalRequestAsync(CreateApprovalRequestDto dto, int approverUserId)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .FirstOrDefaultAsync(lr => lr.Id == dto.LeaveRequestId);

        if (leaveRequest == null)
        {
            return false;
        }

        var approvalRequest = new ApprovalRequest
        {
            ApproverId = approverUserId,
            LeaveRequestId = dto.LeaveRequestId,
            Status = dto.Status,
            Comment = dto.Comment
        };

        if (dto.Status == RequestStatus.Approved)
        {
            leaveRequest.Status = RequestStatus.Approved;

                var employee = leaveRequest.Employee;
            var leaveDuration = (leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
            employee.OutOfOfficeBalance -= (int)leaveDuration;

                _context.Employees.Update(employee);
        }
        else if (dto.Status == RequestStatus.Rejected)
        {
            leaveRequest.Status = RequestStatus.Rejected;
        }

        await _context.ApprovalRequests.AddAsync(approvalRequest);
        _context.LeaveRequests.Update(leaveRequest);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateApprovalRequestAsync(int approvalRequestId, UpdateApprovalRequestDto dto, int approverUserId)
    {
        var approvalRequest = await _context.ApprovalRequests
            .Include(ar => ar.LeaveRequest)
                .ThenInclude(lr => lr.Employee)
            .FirstOrDefaultAsync(ar => ar.Id == approvalRequestId);

        if (approvalRequest == null)
        {
            throw new KeyNotFoundException($"ApprovalRequest with ID {approvalRequestId} not found.");
        }

        var currentStatus = approvalRequest.Status;


        approvalRequest.Comment = dto.Comment;
        approvalRequest.Status = dto.Status;
        approvalRequest.ApproverId = approverUserId;

        if (approvalRequest.LeaveRequest != null)
        {
            approvalRequest.LeaveRequest.Status = dto.Status;

            if (currentStatus == RequestStatus.Pending && dto.Status == RequestStatus.Approved)
            {
                var employee = approvalRequest.LeaveRequest.Employee;
                var leaveDuration = (approvalRequest.LeaveRequest.EndDate - approvalRequest.LeaveRequest.StartDate).TotalDays;
                employee.OutOfOfficeBalance -= (int)leaveDuration;
                _context.Employees.Update(employee);
            }
            else if (currentStatus == RequestStatus.Approved && dto.Status == RequestStatus.Canceled)
            {
                var employee = approvalRequest.LeaveRequest.Employee;
                var leaveDuration = (approvalRequest.LeaveRequest.EndDate - approvalRequest.LeaveRequest.StartDate).TotalDays;
                employee.OutOfOfficeBalance += (int)leaveDuration;
                _context.Employees.Update(employee);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
