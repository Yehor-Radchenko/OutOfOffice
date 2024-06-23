using OutOfOffice.BLL.ViewModels.Employee;

namespace OutOfOffice.BLL.ViewModels.ApprovalRequest
{
    public class BriefApprovalRequestViewModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public BriefEmployeeViewModel ApprovedBy { get; set; }
    }
}
