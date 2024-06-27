using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.Common.ViewModels.ApprovalRequest
{
    public class TableApprovalRequestViewModel
    {
        public int Id { get; set; }

        public int ApproverId { get; set; }

        public int LeaveRequestId { get; set; }

        public string Status { get; set; } = null!;

        public string Comment { get; set; } = null!;
    }
}
