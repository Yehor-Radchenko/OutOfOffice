using OutOfOffice.Common.ViewModels.Employee;

namespace OutOfOffice.Common.ViewModels.Project
{
    public class BriefProjectViewModel
    {
        public int Id { get; set; }

        public BriefEmployeeViewModel ProjectManager { get; set; } = null!;

        public string Comment { get; set; } = null!;

    }
}
