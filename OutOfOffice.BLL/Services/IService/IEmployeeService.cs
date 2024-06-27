using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.Common.Services.IService
{
    internal interface IEmployeeService 
        : IService<EmployeeDto, FullEmployeeViewModel, BriefEmployeeViewModel, TableEmployeeViewModel>
    {
        Task<bool> ChangeStatusAsync(int id, EmployeeStatus expectedEmployeeStatus);
    }
}
