using OutOfOffice.Common.Enums;

namespace OutOfOffice.BLL.ViewModels.Employee
{
    public class TableEmployeeViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public int PositionId { get; set; }

        public string Status { get; set; }

        public int? EmployeePartnerId { get; set; }

        public int OutOfOfficeBalance { get; set; }

        public int? PhotoId { get; set; }
    }
}
