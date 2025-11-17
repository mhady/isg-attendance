using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Settings
{
    public class CreateUpdateCompanySettingsDto
    {
        [Required]
        [Range(1, 24)]
        public double NormalWorkingHours { get; set; }

        [Required]
        public TimeSpan WorkdayStartTime { get; set; }

        [Required]
        public TimeSpan WorkdayEndTime { get; set; }

        [Range(0, 120)]
        public int LateCheckInGracePeriodMinutes { get; set; }

        [Range(0, 120)]
        public int EarlyCheckOutGracePeriodMinutes { get; set; }

        [StringLength(100)]
        public string TimeZone { get; set; } = "UTC";
    }
}
