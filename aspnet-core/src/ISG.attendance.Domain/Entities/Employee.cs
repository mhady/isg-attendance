using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ISG.attendance.Entities
{
    /// <summary>
    /// Represents an employee in the company
    /// </summary>
    public class Employee : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Reference to Identity User (optional)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Employee full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Employee email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Employee phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Location where employee is assigned
        /// </summary>
        public Guid? LocationId { get; set; }

        /// <summary>
        /// Navigation property for Location
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// Employee code or ID
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Is employee active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Navigation property for employee locations (many-to-many)
        /// </summary>
        public virtual ICollection<EmployeeLocation> EmployeeLocations { get; set; }

        protected Employee()
        {
        }

        public Employee(
            Guid id,
            string fullName,
            string email,
            string employeeCode,
            Guid? userId = null,
            Guid? locationId = null,
            string phoneNumber = null,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            EmployeeCode = employeeCode;
            LocationId = locationId;
            PhoneNumber = phoneNumber;
            IsActive = true;
            TenantId = tenantId;
        }

        public void UpdateEmployee(string fullName, string email, string phoneNumber, string employeeCode)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            EmployeeCode = employeeCode;
        }

        public void AssignToLocation(Guid? locationId)
        {
            LocationId = locationId;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
