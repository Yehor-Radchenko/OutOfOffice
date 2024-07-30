using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public string Base64Data { get; set; } = null!;
    }
}
