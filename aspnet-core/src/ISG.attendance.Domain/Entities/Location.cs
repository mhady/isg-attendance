using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ISG.attendance.Entities
{
    /// <summary>
    /// Represents a company location where employees are assigned
    /// </summary>
    public class Location : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location description or address
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is this location active
        /// </summary>
        public bool IsActive { get; set; }

        protected Location()
        {
        }

        public Location(Guid id, string name, string description = null, Guid? tenantId = null)
            : base(id)
        {
            Name = name;
            Description = description;
            IsActive = true;
            TenantId = tenantId;
        }

        public void UpdateLocation(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
