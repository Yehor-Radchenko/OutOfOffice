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
        public AbsenceReason AbsenceReason { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
