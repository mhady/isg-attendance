using System;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Attendances;
using ISG.attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ISG.attendance.Controllers
{
    [RemoteService(Name = "attendance")]
    [Area("attendance")]
    [ControllerName("Attendance")]
    [Route("api/attendance/attendances")]
    public class AttendanceController : AbpController
    {
        private readonly IAttendanceAppService _attendanceAppService;

        public AttendanceController(IAttendanceAppService attendanceAppService)
        {
            _attendanceAppService = attendanceAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<AttendanceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _attendanceAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public virtual Task<AttendanceDto> GetAsync(Guid id)
        {
            return _attendanceAppService.GetAsync(id);
        }

        [HttpPost("check-in")]
        public virtual Task<AttendanceDto> CheckInAsync(CheckInDto input)
        {
            return _attendanceAppService.CheckInAsync(input);
        }

        [HttpPost("check-out")]
        public virtual Task<AttendanceDto> CheckOutAsync(CheckOutDto input)
        {
            return _attendanceAppService.CheckOutAsync(input);
        }

        [HttpGet("today")]
        public virtual Task<AttendanceDto> GetTodayAttendanceAsync()
        {
            return _attendanceAppService.GetTodayAttendanceAsync();
        }

        [HttpGet("by-employee/{employeeId}")]
        public virtual Task<PagedResultDto<AttendanceDto>> GetAttendanceByEmployeeAsync(
            Guid employeeId,
            PagedAndSortedResultRequestDto input)
        {
            return _attendanceAppService.GetAttendanceByEmployeeAsync(employeeId, input);
        }

        [HttpGet("by-date")]
        public virtual Task<PagedResultDto<AttendanceDto>> GetAttendanceByDateAsync(
            [FromQuery] DateTime date,
            [FromQuery] PagedAndSortedResultRequestDto input)
        {
            return _attendanceAppService.GetAttendanceByDateAsync(date, input);
        }

        [HttpGet("by-date-range")]
        public virtual Task<ListResultDto<AttendanceDto>> GetAttendanceByDateRangeAsync(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] Guid? employeeId = null)
        {
            return _attendanceAppService.GetAttendanceByDateRangeAsync(startDate, endDate, employeeId);
        }
    }
}
