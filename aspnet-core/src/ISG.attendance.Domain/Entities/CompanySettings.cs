using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ISG.attendance.Entities
{
    /// <summary>
    /// Company-specific settings for attendance management
    /// </summary>
    public class CompanySettings : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Normal working hours per day
        /// </summary>
        public double NormalWorkingHours { get; set; }

        /// <summary>
        /// Workday start time (e.g., "08:00")
        /// </summary>
        public TimeSpan WorkdayStartTime { get; set; }

        /// <summary>
        /// Workday end time (e.g., "17:00")
        /// </summary>
        public TimeSpan WorkdayEndTime { get; set; }

        /// <summary>
        /// Allow late check-in (minutes grace period)
        /// </summary>
        public int LateCheckInGracePeriodMinutes { get; set; }

        /// <summary>
        /// Allow early check-out (minutes grace period)
        /// </summary>
        public int EarlyCheckOutGracePeriodMinutes { get; set; }

        /// <summary>
        /// Timezone for the company
        /// </summary>
        public string TimeZone { get; set; }

        protected CompanySettings()
        {
        }

        public CompanySettings(
            Guid id,
            double normalWorkingHours,
            TimeSpan workdayStartTime,
            TimeSpan workdayEndTime,
            Guid? tenantId = null)
            : base(id)
        {
            NormalWorkingHours = normalWorkingHours;
            WorkdayStartTime = workdayStartTime;
            WorkdayEndTime = workdayEndTime;
            TenantId = tenantId;
            LateCheckInGracePeriodMinutes = 0;
            EarlyCheckOutGracePeriodMinutes = 0;
            TimeZone = "UTC";
        }

        public void UpdateSettings(
            double normalWorkingHours,
            TimeSpan workdayStartTime,
            TimeSpan workdayEndTime,
            int lateCheckInGracePeriod,
            int earlyCheckOutGracePeriod,
            string timeZone)
        {
            NormalWorkingHours = normalWorkingHours;
            WorkdayStartTime = workdayStartTime;
            WorkdayEndTime = workdayEndTime;
            LateCheckInGracePeriodMinutes = lateCheckInGracePeriod;
            EarlyCheckOutGracePeriodMinutes = earlyCheckOutGracePeriod;
            TimeZone = timeZone;
        }
    }
}
