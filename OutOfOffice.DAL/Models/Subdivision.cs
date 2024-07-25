using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DAL.Models
{
    public class Subdivision
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}