using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.ApprovalRequest;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.ViewModels.LeaveRequest;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using OutOfOffice.BLL.Services.IService;

namespace OutOfOffice.BLL.Services;

public class LeaveRequestService : IRequestService
{
    private readonly OutOfOfficeDbContext _context;

    public LeaveRequestService (OutOfOfficeDbContext context)
    {
        _context = context;
    }

    public async Task<FullLeaveRequestViewModel> GetFullInfoByIdAsync(int id)
    {
        var leaveRequest = await _context.LeaveRequests
        .Include(lr => lr.Employee)
        .Include(lr => lr.AbsenceReason)
        .Include(lr => lr.ApprovalRequest)
            .ThenInclude(ar => ar.Approver)
        .FirstOrDefaultAsync(lr => lr.Id == id);

        if (leaveRequest == null)
        {
            throw new KeyNotFoundException($"LeaveRequest with ID {id} not found.");
        }

        var viewModel = new FullLeaveRequestViewModel
        {
            Id = leaveRequest.Id,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Comment = leaveRequest.Comment,
            Status = leaveRequest.Status.ToString(),
            AbsenceReason = leaveRequest.AbsenceReason.ReasonTitle,
            Employee = new BriefEmployeeViewModel
            {
                Id = leaveRequest.Employee.Id,
                FullName = leaveRequest.Employee.FullName,
            },
            ApprovalRequest = leaveRequest.ApprovalRequest == null ? null : new BriefApprovalRequestViewModel
            {
                Id = leaveRequest.ApprovalRequest.Id,
                Status = leaveRequest.ApprovalRequest.Status.ToString(),
                ApprovedBy = leaveRequest.ApprovalRequest.Approver == null ? null : new BriefEmployeeViewModel
                {
                    Id = leaveRequest.ApprovalRequest.Approver.Id,
                    FullName = leaveRequest.ApprovalRequest.Approver.FullName,
                }
            }
        };

        return viewModel;
    }

    public async Task<List<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveRequestsAsync(int userId, string? searchValue = null)
    {
        var query = _context.LeaveRequests
            .Include(lr => lr.ApprovalRequest)
            .Where(lr => lr.EmployeeId == userId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
        {
            searchValue = searchValue.ToLower();
            query = query.Where(lr =>
                lr.Id.ToString().Contains(searchValue) ||
                lr.Employee.FullName.ToLower().Contains(searchValue));
        }

        var leaveRequests = await query.ToListAsync();

        var viewModelList = leaveRequests.Select(leaveRequest => new EmployeeLeaveRequestViewModel
        {
            Id = leaveRequest.Id,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Comment = leaveRequest.Comment,
            Status = leaveRequest.Status.ToString(),
            AbsenceReason = leaveRequest.AbsenceReason.ReasonTitle,
            ApproveStatus = leaveRequest.ApprovalRequest != null ? leaveRequest.ApprovalRequest.Status.ToString() : RequestStatus.Pending.ToString(),
        }).ToList();

        return viewModelList;
    }

    public async Task<List<TableLeaveRequestViewModel>> GetAllLeaveRequestsAsync(string? searchValue = null)
    {
        var query = _context.LeaveRequests
            .Include(lr => lr.ApprovalRequest)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
        {
            searchValue = searchValue.ToLower();
            query = query.Where(lr =>
                lr.Id.ToString().Contains(searchValue) ||
                lr.Employee.FullName.ToLower().Contains(searchValue));
        }

        var leaveRequests = await query.ToListAsync();

        var viewModelList = leaveRequests.Select(leaveRequest => new TableLeaveRequestViewModel
        {
            Id = leaveRequest.Id,
            EmployeeId = leaveRequest.EmployeeId,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Comment = leaveRequest.Comment,
            Status = leaveRequest.Status.ToString(),
            AbsenceReason = leaveRequest.AbsenceReason.ReasonTitle,
            ApprovalRequestId = leaveRequest.ApprovalRequest?.Id ?? 0,
        }).ToList();

        return viewModelList;
    }

    public async Task<int> AddAsync(int employeeId, LeaveRequestDto requestDto)
    {
        var leaveRequest = new LeaveRequest
        {
            EmployeeId = employeeId,
            StartDate = requestDto.StartDate,
            EndDate = requestDto.EndDate,
            Comment = requestDto.Comment,
            Status = requestDto.Status,
            AbsenceReasonId = requestDto.AbsenceReasonId,
        };

        await _context.LeaveRequests.AddAsync(leaveRequest);
        await _context.SaveChangesAsync();

        return leaveRequest.Id;
    }

    public async Task<bool> UpdateAsync(int id, LeaveRequestDto expectedValues)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
        {
            return false;
        }

        leaveRequest.StartDate = expectedValues.StartDate;
        leaveRequest.EndDate = expectedValues.EndDate;
        leaveRequest.Comment = expectedValues.Comment;
        leaveRequest.Status = expectedValues.Status;
        leaveRequest.AbsenceReasonId = expectedValues.AbsenceReasonId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelRequestAsync(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(lr => lr.ApprovalRequest)
            .FirstOrDefaultAsync(lr => lr.Id == id);

        if (leaveRequest == null)
        {
            return false;
            throw new KeyNotFoundException($"LeaveRequest with ID {id} not found.");
        }

        leaveRequest.Status = RequestStatus.Canceled;

        if (leaveRequest.ApprovalRequest != null)
        {
            leaveRequest.ApprovalRequest.Status = RequestStatus.Canceled;

            if (leaveRequest.ApprovalRequest.Status == RequestStatus.Approved)
            {
                var employee = leaveRequest.Employee;
                var leaveDuration = (leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                employee.OutOfOfficeBalance += (int)leaveDuration;

                _context.Employees.Update(employee);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
