using OutOfOffice.Common.ViewModels.ApprovalRequest;
using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.Common.ViewModels.LeaveRequest
{
    public class FullLeaveRequestViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string AbsenceReason { get; set; } = null!;

        public BriefEmployeeViewModel Employee { get; set; } = null!;

        public BriefApprovalRequestViewModel? ApprovalRequest { get; set; }
    }
}
