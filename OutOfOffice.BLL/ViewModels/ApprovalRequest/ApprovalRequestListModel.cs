using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.ApprovalRequest
{
    public class ApprovalRequestListModel
    {
        public int Id { get; set; }

        public RequestStatus Status { get; set; }

        public  EmployeeListViewModel ApprovedBy { get; set; }
    }
}
