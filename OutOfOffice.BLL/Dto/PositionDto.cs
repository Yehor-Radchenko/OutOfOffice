using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto
{
    public class PositionDto
    {
        [Required(ErrorMessage = "Position name is required.")]
        public string Name { get; set; } = null!;
    }
}
