using System;

namespace ISG.attendance.DTOs.Reports
{
    public class LocationSummaryDto
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int PresentToday { get; set; }
    }
}
