using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.ApprovalRequest
{
    public class TableApprovalRequestViewModel
    {
        public int Id { get; set; }

        public int ApproverId { get; set; }

        public int LeaveRequestId { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; } = null!;
    }
}
