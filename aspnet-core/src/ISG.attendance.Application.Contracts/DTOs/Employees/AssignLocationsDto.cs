using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Employees
{
    public class AssignLocationsDto
    {
        [Required]
        public List<Guid> LocationIds { get; set; } = new List<Guid>();
    }
}
