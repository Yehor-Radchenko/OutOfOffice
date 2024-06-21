using OutOfOffice.BLL.ViewModels.Employee;
using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.Project
{
    public class FullProjectViewModel
    {
        public int Id { get; set; }

        public ProjectType ProjectType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public ProjectStatus Status { get; set; }

        public BriefEmployeeViewModel ProjectManager { get; set; } = null!;

        public IEnumerable<BriefEmployeeViewModel>? InvolvedEmployees { get; set; }
    }
}
