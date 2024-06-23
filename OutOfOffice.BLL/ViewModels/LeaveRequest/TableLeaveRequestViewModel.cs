using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.LeaveRequest
{
    public class TableLeaveRequestViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; }

        public string AbsenceReason { get; set; }

        public int ApprovalRequestId { get; set; }
    }
}
