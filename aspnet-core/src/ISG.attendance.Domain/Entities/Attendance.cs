using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ISG.attendance.Entities
{
    /// <summary>
    /// Represents employee attendance record
    /// </summary>
    public class Attendance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Employee reference
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Navigation property for Employee
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Date of attendance
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Check-in time
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// Check-out time
        /// </summary>
        public DateTime? CheckOutTime { get; set; }

        /// <summary>
        /// Total worked hours
        /// </summary>
        public double TotalHours { get; set; }

        /// <summary>
        /// Overtime hours
        /// </summary>
        public double OvertimeHours { get; set; }

        /// <summary>
        /// Is this a full working day
        /// </summary>
        public bool IsFullDay { get; set; }

        /// <summary>
        /// Notes or remarks
        /// </summary>
        public string Notes { get; set; }

        protected Attendance()
        {
        }

        public Attendance(
            Guid id,
            Guid employeeId,
            DateTime date,
            Guid? tenantId = null)
            : base(id)
        {
            EmployeeId = employeeId;
            Date = date.Date; // Store only the date part
            TenantId = tenantId;
            IsFullDay = false;
            TotalHours = 0;
            OvertimeHours = 0;
        }

        public void CheckIn(DateTime checkInTime)
        {
            CheckInTime = checkInTime;
        }

        public void CheckOut(DateTime checkOutTime, double normalWorkingHours)
        {
            CheckOutTime = checkOutTime;
            CalculateHours(normalWorkingHours);
        }

        private void CalculateHours(double normalWorkingHours)
        {
            if (CheckInTime.HasValue && CheckOutTime.HasValue)
            {
                var timeSpan = CheckOutTime.Value - CheckInTime.Value;
                TotalHours = timeSpan.TotalHours;

                if (TotalHours >= normalWorkingHours)
                {
                    IsFullDay = true;
                    OvertimeHours = TotalHours - normalWorkingHours;
                }
                else
                {
                    IsFullDay = false;
                    OvertimeHours = 0;
                }
            }
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
        }
    }
}
