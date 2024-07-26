using Blazored.LocalStorage;
using OutOfOffice.BlazorUI.Services.Contracts;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.LeaveRequest;
using System.Net.Http.Json;
using System.Text.Json;

namespace OutOfOffice.BlazorUI.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeaveRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<int> AddLeaveRequest(LeaveRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TableLeaveRequestViewModel>> GetAllLeaveRequests(string? searchString = null)
        {
            var queryString = string.IsNullOrEmpty(searchString) ? string.Empty : $"?searchValue={searchString}";

            var response = await _httpClientFactory.CreateClient("API").GetAsync($"api/leaverequest/manager{queryString}");

            var leaveRequests = await response.Content.ReadFromJsonAsync<List<TableLeaveRequestViewModel>>();

            return leaveRequests;
        }

        public Task<FullLeaveRequestViewModel> GetFullLeaveRequestViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetMyLeaveRequests(string? searchString = null)
        {
            var queryString = string.IsNullOrEmpty(searchString) ? string.Empty : $"?searchValue={searchString}";

            var response = await _httpClientFactory.CreateClient("API").GetAsync($"api/leaverequest/employee{queryString}");

            var leaveRequests = await response.Content.ReadFromJsonAsync<List<EmployeeLeaveRequestViewModel>>();
            return leaveRequests;
        }

        public Task<bool> UpdateLeaveRequest(int id, LeaveRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
