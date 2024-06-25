using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.Dto
{
    public class PositionDto
    {
        [Required(ErrorMessage = "Position name is required.")]
        public string Name { get; set; } = null!;
    }
}
