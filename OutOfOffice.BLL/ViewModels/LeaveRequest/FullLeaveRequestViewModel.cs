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

        public RequestStatus Status { get; set; }

        public AbsenceReason AbsenceReason { get; set; }

        public EmployeeListViewModel Employee { get; set; } = null!;

        public ApprovalRequestListModel? ApprovalRequest { get; set; }
    }
}
