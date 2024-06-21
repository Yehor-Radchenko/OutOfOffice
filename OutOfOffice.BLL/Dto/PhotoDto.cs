using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.BLL.Dto
{
    public class PhotoDto
    {
        [Required(ErrorMessage = "Photo not loaded.")]
        public IFormFile Base64Data { get; set; } = null!;
    }
}
