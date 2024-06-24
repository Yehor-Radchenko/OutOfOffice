using Microsoft.AspNetCore.Identity;
using OutOfOffice.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.DAL.Models
{
    public class Employee : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Subdivision { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }

        [ForeignKey(nameof(EmployeePartner))]
        public int? EmployeePartnerId { get; set; }

        [Required]
        public int OutOfOfficeBalance { get; set; }

        [ForeignKey(nameof(Photo))]
        public int? PhotoId { get; set; }

        public Photo Photo { get; set; } = null!;

        public Employee? EmployeePartner { get; set; }

        public Position Position { get; set; } = new Position();

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        public ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();

        public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
