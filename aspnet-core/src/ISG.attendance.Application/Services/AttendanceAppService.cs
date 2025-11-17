using System;
using System.Linq;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Attendances;
using ISG.attendance.Entities;
using ISG.attendance.Permissions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ISG.attendance.Services
{
    public class AttendanceAppService : ApplicationService, IAttendanceAppService
    {
        private readonly IRepository<Attendance, Guid> _attendanceRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<CompanySettings, Guid> _settingsRepository;

        public AttendanceAppService(
            IRepository<Attendance, Guid> attendanceRepository,
            IRepository<Employee, Guid> employeeRepository,
            IRepository<CompanySettings, Guid> settingsRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task<PagedResultDto<AttendanceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.View);

            var queryable = await _attendanceRepository.GetQueryableAsync();
            queryable = queryable
                .Include(a => a.Employee)
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.CheckInTime);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var attendances = await AsyncExecuter.ToListAsync(
                queryable
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var dtos = attendances.Select(MapToDto).ToList();

            return new PagedResultDto<AttendanceDto>(totalCount, dtos);
        }

        public async Task<AttendanceDto> GetAsync(Guid id)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.View);

            var attendance = await _attendanceRepository.GetAsync(id, includeDetails: true);

            return MapToDto(attendance);
        }

        public async Task<AttendanceDto> CheckInAsync(CheckInDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.CheckIn);

            // Get current user's employee record
            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.UserId == CurrentUser.Id);

            if (employee == null)
            {
                throw new UserFriendlyException("Employee record not found");
            }

            var today = input.CheckInTime.Date;

            // Check if already checked in today
            var existingAttendance = await _attendanceRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employee.Id && a.Date == today);

            if (existingAttendance != null && existingAttendance.CheckInTime.HasValue)
            {
                throw new UserFriendlyException("Already checked in today");
            }

            // Ensure check-in is for today only
            if (today != DateTime.Today)
            {
                throw new UserFriendlyException("Can only check in for today");
            }

            Attendance attendance;
            if (existingAttendance != null)
            {
                attendance = existingAttendance;
                attendance.CheckIn(input.CheckInTime);
                attendance.UpdateNotes(input.Notes);
                await _attendanceRepository.UpdateAsync(attendance);
            }
            else
            {
                attendance = new Attendance(
                    GuidGenerator.Create(),
                    employee.Id,
                    today,
                    CurrentTenant.Id
                );

                attendance.CheckIn(input.CheckInTime);
                attendance.UpdateNotes(input.Notes);

                await _attendanceRepository.InsertAsync(attendance);
            }

            return await GetAsync(attendance.Id);
        }

        public async Task<AttendanceDto> CheckOutAsync(CheckOutDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.CheckOut);

            // Get current user's employee record
            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.UserId == CurrentUser.Id);

            if (employee == null)
            {
                throw new UserFriendlyException("Employee record not found");
            }

            var today = input.CheckOutTime.Date;

            // Ensure check-out is for today only
            if (today != DateTime.Today)
            {
                throw new UserFriendlyException("Can only check out for today");
            }

            var attendance = await _attendanceRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employee.Id && a.Date == today);

            if (attendance == null || !attendance.CheckInTime.HasValue)
            {
                throw new UserFriendlyException("Must check in before checking out");
            }

            if (attendance.CheckOutTime.HasValue)
            {
                throw new UserFriendlyException("Already checked out today");
            }

            // Get company settings for normal working hours
            var settings = await _settingsRepository.FirstOrDefaultAsync();
            var normalWorkingHours = settings?.NormalWorkingHours ?? 8.0;

            attendance.CheckOut(input.CheckOutTime, normalWorkingHours);

            if (!string.IsNullOrWhiteSpace(input.Notes))
            {
                attendance.UpdateNotes(attendance.Notes + " | " + input.Notes);
            }

            await _attendanceRepository.UpdateAsync(attendance);

            return await GetAsync(attendance.Id);
        }

        public async Task<AttendanceDto> GetTodayAttendanceAsync()
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.ViewOwn);

            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.UserId == CurrentUser.Id);

            if (employee == null)
            {
                throw new UserFriendlyException("Employee record not found");
            }

            var today = DateTime.Today;
            var attendance = await _attendanceRepository.FirstOrDefaultAsync(a =>
                a.EmployeeId == employee.Id && a.Date == today);

            return attendance != null ? MapToDto(attendance) : null;
        }

        public async Task<PagedResultDto<AttendanceDto>> GetAttendanceByEmployeeAsync(
            Guid employeeId,
            PagedAndSortedResultRequestDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.View);

            var queryable = await _attendanceRepository.GetQueryableAsync();
            queryable = queryable
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var attendances = await AsyncExecuter.ToListAsync(
                queryable
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var dtos = attendances.Select(MapToDto).ToList();

            return new PagedResultDto<AttendanceDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<AttendanceDto>> GetAttendanceByDateAsync(
            DateTime date,
            PagedAndSortedResultRequestDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.View);

            var queryable = await _attendanceRepository.GetQueryableAsync();
            queryable = queryable
                .Include(a => a.Employee)
                .Where(a => a.Date == date.Date)
                .OrderBy(a => a.Employee.FullName);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var attendances = await AsyncExecuter.ToListAsync(
                queryable
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var dtos = attendances.Select(MapToDto).ToList();

            return new PagedResultDto<AttendanceDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<AttendanceDto>> GetAttendanceByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? employeeId = null)
        {
            await CheckPolicyAsync(attendancePermissions.Attendances.View);

            var queryable = await _attendanceRepository.GetQueryableAsync();
            queryable = queryable
                .Include(a => a.Employee)
                .Where(a => a.Date >= startDate.Date && a.Date <= endDate.Date);

            if (employeeId.HasValue)
            {
                queryable = queryable.Where(a => a.EmployeeId == employeeId.Value);
            }

            queryable = queryable.OrderByDescending(a => a.Date);

            var attendances = await AsyncExecuter.ToListAsync(queryable);
            var dtos = attendances.Select(MapToDto).ToList();

            return new ListResultDto<AttendanceDto>(dtos);
        }

        private AttendanceDto MapToDto(Attendance attendance)
        {
            return new AttendanceDto
            {
                Id = attendance.Id,
                EmployeeId = attendance.EmployeeId,
                EmployeeName = attendance.Employee?.FullName,
                EmployeeCode = attendance.Employee?.EmployeeCode,
                Date = attendance.Date,
                CheckInTime = attendance.CheckInTime,
                CheckOutTime = attendance.CheckOutTime,
                TotalHours = attendance.TotalHours,
                OvertimeHours = attendance.OvertimeHours,
                IsFullDay = attendance.IsFullDay,
                Notes = attendance.Notes,
                CreationTime = attendance.CreationTime
            };
        }

        private async Task CheckPolicyAsync(string policyName)
        {
            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
