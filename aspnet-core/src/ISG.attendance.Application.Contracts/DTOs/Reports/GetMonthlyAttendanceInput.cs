using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Reports
{
    public class GetMonthlyAttendanceInput
    {
        public Guid? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }
    }
}
