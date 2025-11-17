using System;
using Volo.Abp.Application.Dtos;

namespace ISG.attendance.DTOs.Employees
{
    public class EmployeeDto : AuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EmployeeCode { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; }
        public bool IsActive { get; set; }
    }
}
