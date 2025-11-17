using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string EmployeeCode { get; set; }

        public Guid? LocationId { get; set; }

        public bool IsActive { get; set; }
    }
}
