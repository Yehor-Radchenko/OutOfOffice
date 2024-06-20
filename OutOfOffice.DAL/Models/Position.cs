using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
