using OutOfOffice.Common.Enums;
using OutOfOffice.BLL.ViewModels.LeaveRequest;

namespace OutOfOffice.BLL.ViewModels.Employee
{
    public class FullEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public int PositionId { get; set; }

        public EmployeeStatus Status { get; set; }

        public int OutOfOfficeBalance { get; set; }

        public string Photo { get; set; }

        public EmployeeListViewModel EmployeePartnerInfo { get; set; }

        public string Position { get; set; }
    }
}
