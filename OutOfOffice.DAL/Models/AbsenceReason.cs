using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.Models
{
    public class AbsenceReason
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string ReasonTitle { get; set; } = string.Empty;

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
