using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.LeaveRequest;

namespace OutOfOffice.BlazorUI.Services.Contracts
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetMyLeaveRequestsAsync(string? searchString = null);

        Task<IEnumerable<TableLeaveRequestViewModel>> GetAllLeaveRequestsAsync(string? searchString = null);

        Task<FullLeaveRequestViewModel> GetFullLeaveRequestViewModelAsync(int id);

        Task AddAsync(LeaveRequestDto dto);

        Task UpdateAsync(int id, LeaveRequestDto dto);

        Task<bool> CancelLeaveRequest(int id);
    }
}
