using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Attendances
{
    public class CheckInDto
    {
        [Required]
        public DateTime CheckInTime { get; set; }

        public string Notes { get; set; }
    }
}
