using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.ApprovalRequest
{
    public class BriefApprovalRequestViewModel
    {
        public int Id { get; set; }

        public RequestStatus Status { get; set; }

        public  BriefEmployeeViewModel ApprovedBy { get; set; }
    }
}
