using System;

namespace ISG.attendance.DTOs.Employees
{
    public class ImportEmployeeDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
    }
}
