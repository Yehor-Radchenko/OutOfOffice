using OutOfOffice.Common.ValidationAttributes;
using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.Dto
{
    public class LeaveRequestDto
    {
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;

        [Required(ErrorMessage = "RequestStatus is required.")]
        public RequestStatus Status { get; set; }

        [Required(ErrorMessage = "AbsenceReason is required.")]
        [ValidAbsenceReason(ErrorMessage = "The provided Absence reason is not valid.")]
        public string AbsenceReason { get; set; }
    }
}
