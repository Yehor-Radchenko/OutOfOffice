using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.Common.ViewModels.ApprovalRequest
{
    public class BriefApprovalRequestViewModel
    {
        public int Id { get; set; }

        public string Status { get; set; } = null!;

        public BriefEmployeeViewModel ApprovedBy { get; set; } = null!;
    }
}
