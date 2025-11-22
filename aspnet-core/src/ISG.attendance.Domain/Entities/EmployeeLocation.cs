using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ISG.attendance.Entities
{
    /// <summary>
    /// Represents the many-to-many relationship between Employee and Location
    /// Allows an employee to be assigned to multiple locations
    /// </summary>
    public class EmployeeLocation : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Navigation property for Employee
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Navigation property for Location
        /// </summary>
        public virtual Location Location { get; set; }

        protected EmployeeLocation()
        {
        }

        public EmployeeLocation(
            Guid id,
            Guid employeeId,
            Guid locationId,
            Guid? tenantId = null)
            : base()
        {
            Id = id;
            EmployeeId = employeeId;
            LocationId = locationId;
            TenantId = tenantId;
        }
    }
}
