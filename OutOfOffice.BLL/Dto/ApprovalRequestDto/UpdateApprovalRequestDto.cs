using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto.ApprovalRequestDto
{
    public class UpdateApprovalRequestDto
    {
        [Required(ErrorMessage = "RequestStatus is required.")]
        public RequestStatus Status { get; set; }

        public string Comment { get; set; } = null!;
    }
}
