using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.Employee
{
    public class TableEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public int PositionId { get; set; }

        public EmployeeStatus Status { get; set; }

        public int OutOfOfficeBalance { get; set; }

        public string PartnerFullName { get; set; }

        public string Position { get; set; }
    }
}
