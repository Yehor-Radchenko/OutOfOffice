using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class ApprovalRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Approver))]
        public int ApproverId { get; set; }

        [Required]
        [ForeignKey(nameof(LeaveRequest))]
        public int LeaveRequestId { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        public string Comment { get; set; } = null!;    

        public Employee Approver { get; set; } = null!;

        public LeaveRequest LeaveRequest { get; set; } = null!;
    }
}
