using OutOfOffice.BLL.ViewModels.ApprovalRequest;
using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.LeaveRequest
{
    public class EmployeeLeaveRequestViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public RequestStatus LeaveRequestStatus { get; set; }

        public AbsenceReason AbsenceReason { get; set; }

        public RequestStatus? ApproveStatus { get; set; } = null;
    }
}
