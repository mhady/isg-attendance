using System;
using Volo.Abp.Application.Dtos;

namespace ISG.attendance.DTOs.Settings
{
    public class CompanySettingsDto : AuditedEntityDto<Guid>
    {
        public double NormalWorkingHours { get; set; }
        public TimeSpan WorkdayStartTime { get; set; }
        public TimeSpan WorkdayEndTime { get; set; }
        public int LateCheckInGracePeriodMinutes { get; set; }
        public int EarlyCheckOutGracePeriodMinutes { get; set; }
        public string TimeZone { get; set; }
    }
}
