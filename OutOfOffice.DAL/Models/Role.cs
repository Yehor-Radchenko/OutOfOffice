using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        public string Name { get; set; } = null!;
    }
}
