using System;
using Volo.Abp.Application.Dtos;

namespace ISG.attendance.DTOs.Locations
{
    public class LocationDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeCount { get; set; }
    }
}
