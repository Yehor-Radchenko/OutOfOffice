using OutOfOffice.Common.ViewModels.LeaveRequest;

namespace OutOfOffice.Common.ViewModels.Employee
{
    public class FullEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public string Status { get; set; }

        public int OutOfOfficeBalance { get; set; }

        public BriefEmployeeViewModel? EmployeePartnerInfo { get; set; }

        public string Position { get; set; }

        public IEnumerable<BriefEmployeeViewModel>? SubordinateEmployees { get; set; }

        public IEnumerable<EmployeeLeaveRequestViewModel>? LeaveRequests { get; set; }

        public IEnumerable<int>? ProjectIds { get; set; }

        public byte[] Photo { get; set; }
    }
}
