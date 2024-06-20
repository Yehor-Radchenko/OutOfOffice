using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public byte[] Base64Data { get; set; } = null!;
    }
}
