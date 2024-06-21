using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Subdivision is required.")]
        public string Subdivision { get; set; } = null!;

        [Required(ErrorMessage = "Position is required.")]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "EmployeeStatus is required.")]
        public EmployeeStatus Status { get; set; }

        [Required(ErrorMessage = "EmployeePartner is required.")]
        public int EmployeePartnerId { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        public int OutOfOfficeBalance { get; set; }

        public int PhotoId { get; set; }

        public IEnumerable<int> SubordinateEmployeeIds { get; set; } = null!;

        public IEnumerable<int> LeaveRequestIds { get; set; } = null!;

        public IEnumerable<int> ApprovalRequestIds { get; set; } = null!;

        public IEnumerable<int> ProjectsAsProjectManagerIds { get; set; } = null!;
    }
}
