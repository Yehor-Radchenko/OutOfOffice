using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.Dto
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Subdivision is required.")]
        public int SubdivisionId { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "EmployeePartner is required.")]
        public int? EmployeePartnerId { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        public int OutOfOfficeBalance { get; set; }

        public string? PhotoBase64 { get; set; }
    }
}
