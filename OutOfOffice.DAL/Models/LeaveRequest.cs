using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        [Required]
        [ForeignKey(nameof(AbsenceReason))]
        public int AbsenceReasonId { get; set; }

        public AbsenceReason AbsenceReason { get; set; } = null!;

        public Employee Employee { get; set; } = null!;

        public ApprovalRequest? ApprovalRequest { get; set; }
    }
}
