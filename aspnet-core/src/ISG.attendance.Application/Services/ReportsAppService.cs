using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Reports;
using ISG.attendance.Entities;
using ISG.attendance.Permissions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ISG.attendance.Services
{
    public class ReportsAppService : ApplicationService, IReportsAppService
    {
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<Attendance, Guid> _attendanceRepository;

        public ReportsAppService(
            IRepository<Location, Guid> locationRepository,
            IRepository<Employee, Guid> employeeRepository,
            IRepository<Attendance, Guid> attendanceRepository)
        {
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<List<LocationSummaryDto>> GetLocationSummaryAsync()
        {
            await CheckPolicyAsync(attendancePermissions.Reports.LocationSummary);

            var locations = await _locationRepository.GetListAsync();
            var today = DateTime.Today;

            var summary = new List<LocationSummaryDto>();

            foreach (var location in locations)
            {
                var totalEmployees = await _employeeRepository.CountAsync(e => e.LocationId == location.Id);
                var activeEmployees = await _employeeRepository.CountAsync(e => e.LocationId == location.Id && e.IsActive);

                // Get employees who checked in today
                var employeeIds = (await _employeeRepository.GetListAsync(e => e.LocationId == location.Id && e.IsActive))
                    .Select(e => e.Id)
                    .ToList();

                var presentToday = await _attendanceRepository.CountAsync(a =>
                    employeeIds.Contains(a.EmployeeId) &&
                    a.Date == today &&
                    a.CheckInTime.HasValue);

                summary.Add(new LocationSummaryDto
                {
                    LocationId = location.Id,
                    LocationName = location.Name,
                    TotalEmployees = totalEmployees,
                    ActiveEmployees = activeEmployees,
                    PresentToday = presentToday
                });
            }

            return summary;
        }

        public async Task<List<MonthlyAttendanceReportDto>> GetMonthlyAttendanceReportAsync(GetMonthlyAttendanceInput input)
        {
            await CheckPolicyAsync(attendancePermissions.Reports.MonthlyAttendance);

            var startDate = new DateTime(input.Year, input.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Get employees based on filters
            var employeeQuery = await _employeeRepository.GetQueryableAsync();
            employeeQuery = employeeQuery.Include(e => e.Location);

            if (input.EmployeeId.HasValue)
            {
                employeeQuery = employeeQuery.Where(e => e.Id == input.EmployeeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.EmployeeName))
            {
                employeeQuery = employeeQuery.Where(e => e.FullName.Contains(input.EmployeeName));
            }

            var employees = await AsyncExecuter.ToListAsync(employeeQuery);

            var reports = new List<MonthlyAttendanceReportDto>();

            foreach (var employee in employees)
            {
                var attendances = await _attendanceRepository.GetListAsync(a =>
                    a.EmployeeId == employee.Id &&
                    a.Date >= startDate &&
                    a.Date <= endDate);

                var fullWorkingDays = attendances.Count(a => a.IsFullDay);
                var partialWorkingDays = attendances.Count(a => !a.IsFullDay && a.CheckInTime.HasValue);
                var totalWorkingDays = attendances.Count(a => a.CheckInTime.HasValue);
                var totalHours = attendances.Sum(a => a.TotalHours);
                var totalOvertimeHours = attendances.Sum(a => a.OvertimeHours);

                // Calculate working days in the month (excluding weekends)
                var workingDaysInMonth = 0;
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        workingDaysInMonth++;
                    }
                }

                var absentDays = workingDaysInMonth - totalWorkingDays;

                reports.Add(new MonthlyAttendanceReportDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    EmployeeCode = employee.EmployeeCode,
                    LocationName = employee.Location?.Name,
                    Year = input.Year,
                    Month = input.Month,
                    TotalWorkingDays = totalWorkingDays,
                    FullWorkingDays = fullWorkingDays,
                    PartialWorkingDays = partialWorkingDays,
                    AbsentDays = absentDays,
                    TotalHours = totalHours,
                    TotalOvertimeHours = totalOvertimeHours
                });
            }

            return reports;
        }

        private async Task CheckPolicyAsync(string policyName)
        {
            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
