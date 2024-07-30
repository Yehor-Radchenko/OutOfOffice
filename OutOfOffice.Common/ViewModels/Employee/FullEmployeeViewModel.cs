using OutOfOffice.Common.ViewModels.LeaveRequest;
using OutOfOffice.Common.ViewModels.Project;

namespace OutOfOffice.Common.ViewModels.Employee
{
    public class FullEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int OutOfOfficeBalance { get; set; }

        public BriefEmployeeViewModel? EmployeePartnerInfo { get; set; }

        public string Position { get; set; } = string.Empty;

        public IEnumerable<BriefEmployeeViewModel>? SubordinateEmployees { get; set; }

        public IEnumerable<EmployeeLeaveRequestViewModel>? LeaveRequests { get; set; }

        public IEnumerable<BriefProjectViewModel>? Projects { get; set; }

        public string? PhotoBase64 { get; set; }
    }
}
