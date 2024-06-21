using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Subdivision { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(EmployeePartner))]
        public int EmployeePartnerId { get; set; }

        [Required]
        public int OutOfOfficeBalance { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoId { get; set; }

        public Photo Photo { get; set; } = null!;

        public Employee EmployeePartner { get; set; } = null!;

        public Position Position { get; set; } = null!;

        public ICollection<Employee> SubordinateEmployees { get; set; } = null!;

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = null!;

        public ICollection<ApprovalRequest> ApprovalRequests { get; set; } = null!;

        public ICollection<Project> ProjectsAsProjectManager { get; set; } = null!;
    }
}
