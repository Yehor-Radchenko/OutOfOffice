using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BlazorUI.Pages.LeaveRequest.Models
{
    public class LeaveRequestFormModel
    {
        [Required(ErrorMessage = "AbsenceReason is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid reason.")]
        public int AbsenceReasonId { get; set; }

        public string Comment { get; set; } = null!;

        public DateRange DateRange { get; set; }
    }
}
