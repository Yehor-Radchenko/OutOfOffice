using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.AbsenceReason;

namespace OutOfOffice.BlazorUI.Services.Contracts
{
    public interface IAbsenceReasonService
    {
        Task<IEnumerable<AbsenceReasonViewModel>> GetAllAsync();

        Task<int> AddAsync(AbsenceReasonDto dto);

        Task<int> UpdateAsync(int id, AbsenceReasonDto dto);
    }
}
