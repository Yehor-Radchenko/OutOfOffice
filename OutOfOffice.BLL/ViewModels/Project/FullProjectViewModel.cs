using OutOfOffice.BLL.ViewModels.Employee;

namespace OutOfOffice.BLL.ViewModels.Project
{
    public class FullProjectViewModel
    {
        public int Id { get; set; }

        public string ProjectType { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Comment { get; set; } = null!;

        public string Status { get; set; } = null!;

        public BriefEmployeeViewModel ProjectManager { get; set; } = null!;

        public IEnumerable<BriefEmployeeViewModel>? InvolvedEmployees { get; set; }
    }
}
