using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ProjectType ProjectType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [ForeignKey(nameof(ProjectManager))]
        public int ProjectManagerId { get; set; }

        public string Comment { get; set; } = null!;

        [Required]
        public ProjectStatus Status { get; set; }

        public Employee ProjectManager { get; set; } = null!;
    }
}
