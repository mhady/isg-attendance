using System;
using Volo.Abp.Application.Dtos;

namespace ISG.attendance.DTOs.Attendances
{
    public class AttendanceDto : AuditedEntityDto<Guid>
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public double TotalHours { get; set; }
        public double OvertimeHours { get; set; }
        public bool IsFullDay { get; set; }
        public string Notes { get; set; }
    }
}
