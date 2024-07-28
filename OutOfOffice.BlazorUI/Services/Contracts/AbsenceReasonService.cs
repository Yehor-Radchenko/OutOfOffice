using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.AbsenceReason;
using OutOfOffice.Common.ViewModels.LeaveRequest;
using System.Net.Http.Json;

namespace OutOfOffice.BlazorUI.Services.Contracts
{
    public class AbsenceReasonService : IAbsenceReasonService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AbsenceReasonService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<int> AddAsync(AbsenceReasonDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AbsenceReasonViewModel>> GetAllAsync()
        {
            var response = await _httpClientFactory.CreateClient("API").GetAsync("api/absencereason");

            var reasons = await response.Content.ReadFromJsonAsync<List<AbsenceReasonViewModel>>();

            return reasons;
        }

        public Task<int> UpdateAsync(int id, AbsenceReasonDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
