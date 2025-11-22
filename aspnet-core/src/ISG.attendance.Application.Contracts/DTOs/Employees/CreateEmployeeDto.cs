using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(256)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        public bool CreateUserAccount { get; set; } = false;

        [StringLength(256)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string EmployeeCode { get; set; }

        public Guid? LocationId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
