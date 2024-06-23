using OutOfOffice.BLL.ViewModels.ApprovalRequest;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.LeaveRequest
{
    public class FullLeaveRequestViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; }

        public string AbsenceReason { get; set; }

        public BriefEmployeeViewModel Employee { get; set; } = null!;

        public BriefApprovalRequestViewModel? ApprovalRequest { get; set; }
    }
}
