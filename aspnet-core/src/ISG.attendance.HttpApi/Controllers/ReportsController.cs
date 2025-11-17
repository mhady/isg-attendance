using System.Collections.Generic;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Reports;
using ISG.attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers
{
    [RemoteService(Name = "attendance")]
    [Area("attendance")]
    [ControllerName("Reports")]
    [Route("api/attendance/reports")]
    public class ReportsController : AbpController
    {
        private readonly IReportsAppService _reportsAppService;

        public ReportsController(IReportsAppService reportsAppService)
        {
            _reportsAppService = reportsAppService;
        }

        [HttpGet("location-summary")]
        public virtual Task<List<LocationSummaryDto>> GetLocationSummaryAsync()
        {
            return _reportsAppService.GetLocationSummaryAsync();
        }

        [HttpPost("monthly-attendance")]
        public virtual Task<List<MonthlyAttendanceReportDto>> GetMonthlyAttendanceReportAsync(
            GetMonthlyAttendanceInput input)
        {
            return _reportsAppService.GetMonthlyAttendanceReportAsync(input);
        }
    }
}
