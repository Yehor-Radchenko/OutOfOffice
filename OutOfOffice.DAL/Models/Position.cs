using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
