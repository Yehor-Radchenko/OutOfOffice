using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.BLL.Services.IService
{
    internal interface IEmployeeService 
        : IService<EmployeeDto, FullEmployeeViewModel, BriefEmployeeViewModel, TableEmployeeViewModel>
    {
        Task<bool> ChangeStatusAsync(int id, EmployeeStatus expectedEmployeeStatus);
    }
}
