using System;

namespace ISG.attendance.DTOs.Employees
{
    public class EmployeeLocationDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
