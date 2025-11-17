using System;
using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Attendances
{
    public class CheckOutDto
    {
        [Required]
        public DateTime CheckOutTime { get; set; }

        public string Notes { get; set; }
    }
}
