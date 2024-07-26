using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.LeaveRequest;

namespace OutOfOffice.BlazorUI.Services.Contracts
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetMyLeaveRequests(string? searchString = null);

        Task<IEnumerable<TableLeaveRequestViewModel>> GetAllLeaveRequests(string? searchString = null);

        Task<FullLeaveRequestViewModel> GetFullLeaveRequestViewModel(int id);

        Task<int> AddLeaveRequest(LeaveRequestDto dto);

        Task<bool> UpdateLeaveRequest(int id, LeaveRequestDto dto);

        Task<bool> CancelLeaveRequest(int id);
    }
}
