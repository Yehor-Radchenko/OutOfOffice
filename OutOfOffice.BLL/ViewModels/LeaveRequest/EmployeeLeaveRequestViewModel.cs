using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.LeaveRequest
{
    public class EmployeeLeaveRequestViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string AbsenceReason { get; set; } = null!;

        public string? ApproveStatus { get; set; } = null;
    }
}
