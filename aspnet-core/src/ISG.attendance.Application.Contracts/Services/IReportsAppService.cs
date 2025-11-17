using System.Collections.Generic;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Reports;
using Volo.Abp.Application.Services;

namespace ISG.attendance.Services
{
    public interface IReportsAppService : IApplicationService
    {
        Task<List<LocationSummaryDto>> GetLocationSummaryAsync();

        Task<List<MonthlyAttendanceReportDto>> GetMonthlyAttendanceReportAsync(GetMonthlyAttendanceInput input);
    }
}
