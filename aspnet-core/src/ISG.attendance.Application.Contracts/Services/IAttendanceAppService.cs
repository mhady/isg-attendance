using System;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Attendances;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ISG.attendance.Services
{
    public interface IAttendanceAppService : IApplicationService
    {
        Task<PagedResultDto<AttendanceDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<AttendanceDto> GetAsync(Guid id);

        Task<AttendanceDto> CheckInAsync(CheckInDto input);

        Task<AttendanceDto> CheckOutAsync(CheckOutDto input);

        Task<AttendanceDto> GetTodayAttendanceAsync();

        Task<PagedResultDto<AttendanceDto>> GetAttendanceByEmployeeAsync(Guid employeeId, PagedAndSortedResultRequestDto input);

        Task<PagedResultDto<AttendanceDto>> GetAttendanceByDateAsync(DateTime date, PagedAndSortedResultRequestDto input);

        Task<ListResultDto<AttendanceDto>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? employeeId = null);
    }
}
