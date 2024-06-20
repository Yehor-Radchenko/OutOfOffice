using OutOfOffice.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [ForeignKey("Position")]
        public int PositionId { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }

        [Required]
        [ForeignKey("EmployeePartner")]
        public int EmployeePartnerId { get; set; }

        [Required]
        public int OutOfOfficeBalance { get; set; }

        public int PhotoId { get; set; }

        public Photo Photo { get; set; } = null!;

        public Employee EmployeePartner { get; set; } = null!;

        public Position Position { get; set; } = null!;
    }
}
