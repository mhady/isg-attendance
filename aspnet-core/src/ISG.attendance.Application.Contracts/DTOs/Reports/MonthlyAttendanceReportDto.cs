using System;

namespace ISG.attendance.DTOs.Reports
{
    public class MonthlyAttendanceReportDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalWorkingDays { get; set; }
        public int FullWorkingDays { get; set; }
        public int PartialWorkingDays { get; set; }
        public int AbsentDays { get; set; }
        public double TotalHours { get; set; }
        public double TotalOvertimeHours { get; set; }
    }
}
