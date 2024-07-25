using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.Dto
{
    public class SubdivisionDto
    {
        [Required(ErrorMessage = "Subdivision name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}
