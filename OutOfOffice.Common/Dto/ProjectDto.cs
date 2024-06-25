using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Common.Dto
{
    public class ProjectDto
    {
        [Required(ErrorMessage = "ProjectType is required.")]
        public ProjectType ProjectType { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "ProjectManager is required.")]
        public int ProjectManagerId { get; set; }

        public string Comment { get; set; } = null!;

        [Required(ErrorMessage = "ProjectStatus is required.")]
        public ProjectStatus Status { get; set; }
    }
}
