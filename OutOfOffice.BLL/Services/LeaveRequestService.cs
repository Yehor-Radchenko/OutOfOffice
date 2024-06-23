using Microsoft.EntityFrameworkCore;
using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.Services.IService;
using OutOfOffice.BLL.ViewModels.ApprovalRequest;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.BLL.ViewModels.LeaveRequest;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using System.Collections.Generic;

namespace OutOfOffice.BLL.Services
{
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
                AbsenceReason = leaveRequest.AbsenceReason.ToString(),
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

        public async Task<List<EmployeeLeaveRequestViewModel>> GetTableDataAsync(string? searchValue = null)
        {
            //TODO: Select LeaveRequests only authentificated Employee

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

            var viewModelList = leaveRequests.Select(leaveRequest => new EmployeeLeaveRequestViewModel
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Comment = leaveRequest.Comment,
                Status = leaveRequest.Status.ToString(),
                AbsenceReason = leaveRequest.AbsenceReason.ToString(),
                ApproveStatus = leaveRequest.ApprovalRequest != null ? leaveRequest.ApprovalRequest.Status.ToString() : RequestStatus.Pending.ToString(),
            })
                .ToList();

            return viewModelList;
        }

        public async Task<List<TableLeaveRequestViewModel>> GetTableDataWithEmployeeAsync(string? searchValue = null)
        {
            var query = _context.LeaveRequests.AsQueryable();

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
                AbsenceReason = leaveRequest.AbsenceReason.ToString(),
                ApprovalRequestId = leaveRequest.ApprovalRequestId
            }).ToList();

            return viewModelList;
        }

        public async Task<int> AddAsync(LeaveRequestDto expectedValues)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = expectedValues.EmployeeId,
                StartDate = expectedValues.StartDate,
                EndDate = expectedValues.EndDate,
                Comment = expectedValues.Comment,
                Status = expectedValues.Status,
                AbsenceReason = expectedValues.AbsenceReason,
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
            leaveRequest.AbsenceReason = expectedValues.AbsenceReason;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelRequestAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            leaveRequest.Status = RequestStatus.Canceled;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
