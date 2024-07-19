using OutOfOffice.Common.Dto;
using OutOfOffice.Common.Enums;
using OutOfOffice.Common.ViewModels.Employee;

namespace BlazorWebAssemblyUI.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<int> ChangeStatusAsync(int id, EmployeeStatus status);
        Task<int> CreateEmployeeAsync(EmployeeDto dto);
        Task<FullEmployeeViewModel> GetFullInfoAboutAuthenticatedEmployeeAsync();
        Task<FullEmployeeViewModel> GetFullInfoAsync(int id);
        Task<List<TableEmployeeViewModel>> GetTableDataAsync(string? search = null);
        Task<int> UpdateEmployeeAsync(int id, EmployeeDto dto);
    }
}