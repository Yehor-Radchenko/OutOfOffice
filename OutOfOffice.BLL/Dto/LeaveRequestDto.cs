using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto
{
    public class LeaveRequestDto
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        [Required(ErrorMessage = "RequestStatus is required.")]
        public RequestStatus Status { get; set; }

        [Required(ErrorMessage = "AbsenceReason is required.")]
        public AbsenceReason AbsenceReason { get; set; }

        public IEnumerable<int> ApprovalRequests { get; set; } = null!;
    }
}
