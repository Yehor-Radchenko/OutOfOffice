using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto.ApprovalRequestDto
{
    public class CreateApprovalRequestDto
    {
        [Required(ErrorMessage = "LeaveRequest is required.")]
        public int LeaveRequestId { get; set; }

        [Required(ErrorMessage = "RequestStatus is required.")]
        public RequestStatus Status { get; set; }

        public string Comment { get; set; } = null!;
    }
}
