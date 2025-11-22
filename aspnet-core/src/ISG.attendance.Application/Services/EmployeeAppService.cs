using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISG.attendance.DTOs.Employees;
using ISG.attendance.Entities;
using ISG.attendance.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace ISG.attendance.Services
{
    public class EmployeeAppService : ApplicationService, IEmployeeAppService
    {
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly IRepository<EmployeeLocation, Guid> _employeeLocationRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;

        public EmployeeAppService(
            IRepository<Employee, Guid> employeeRepository,
            IRepository<Location, Guid> locationRepository,
            IRepository<EmployeeLocation, Guid> employeeLocationRepository,
            IIdentityUserRepository userRepository,
            IdentityUserManager userManager)
        {
            _employeeRepository = employeeRepository;
            _locationRepository = locationRepository;
            _employeeLocationRepository = employeeLocationRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<PagedResultDto<EmployeeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Default);

            var queryable = await _employeeRepository.GetQueryableAsync();
            queryable = queryable
                .Include(e => e.Location)
                .Include(e => e.EmployeeLocations)
                .OrderByDescending(e => e.CreationTime);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var employees = await AsyncExecuter.ToListAsync(
                queryable
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                UserId = e.UserId,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                EmployeeCode = e.EmployeeCode,
                LocationId = e.LocationId,
                LocationName = e.Location?.Name,
                IsActive = e.IsActive,
                CreationTime = e.CreationTime,
                LocationIds = e.EmployeeLocations?.Select(el => el.LocationId).ToList() ?? new List<Guid>()
            }).ToList();

            return new PagedResultDto<EmployeeDto>(totalCount, employeeDtos);
        }

        public async Task<EmployeeDto> GetAsync(Guid id)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Default);

            var queryable = await _employeeRepository.GetQueryableAsync();
            var employee = await AsyncExecuter.FirstOrDefaultAsync(
                queryable
                    .Include(e => e.Location)
                    .Include(e => e.EmployeeLocations)
                    .Where(e => e.Id == id)
            );

            if (employee == null)
            {
                throw new EntityNotFoundException(typeof(Employee), id);
            }

            var dto = ObjectMapper.Map<Employee, EmployeeDto>(employee);
            dto.LocationIds = employee.EmployeeLocations?.Select(el => el.LocationId).ToList() ?? new List<Guid>();

            return dto;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Create);

            Guid? userId = null;

            // Create Identity User only if CreateUserAccount is true
            if (input.CreateUserAccount)
            {
                if (string.IsNullOrWhiteSpace(input.Password))
                {
                    throw new UserFriendlyException("Password is required when creating a user account");
                }

                var user = new IdentityUser(
                    GuidGenerator.Create(),
                    input.Email,
                    input.Email,
                    CurrentTenant.Id
                );

                await _userManager.SetEmailAsync(user, input.Email);
                var result = await _userManager.CreateAsync(user, input.Password);

                if (!result.Succeeded)
                {
                    throw new UserFriendlyException(result.Errors.First().Description);
                }

                // Assign Employee role
                await _userManager.AddToRoleAsync(user, "Employee");
                userId = user.Id;
            }

            // Create Employee entity
            var employee = new Employee(
                GuidGenerator.Create(),
                input.FullName,
                input.Email,
                input.EmployeeCode,
                userId,
                input.LocationId,
                input.PhoneNumber,
                CurrentTenant.Id
            );

            employee.SetActive(input.IsActive);

            await _employeeRepository.InsertAsync(employee);

            return await GetAsync(employee.Id);
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Edit);

            var employee = await _employeeRepository.GetAsync(id);

            employee.UpdateEmployee(input.FullName, input.Email, input.PhoneNumber, input.EmployeeCode);
            employee.AssignToLocation(input.LocationId);
            employee.SetActive(input.IsActive);

            // Update user email if user account exists
            if (employee.UserId.HasValue)
            {
                var user = await _userRepository.GetAsync(employee.UserId.Value);
                await _userManager.SetEmailAsync(user, input.Email);
                await _userManager.SetUserNameAsync(user, input.Email);
            }

            await _employeeRepository.UpdateAsync(employee);

            return await GetAsync(employee.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Delete);

            var employee = await _employeeRepository.GetAsync(id);

            // Delete the associated user if exists
            if (employee.UserId.HasValue)
            {
                var user = await _userRepository.FindAsync(employee.UserId.Value);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<List<EmployeeDto>> ImportFromExcelAsync(List<ImportEmployeeDto> employees)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Import);

            var createdEmployees = new List<EmployeeDto>();
            var locations = await _locationRepository.GetListAsync();

            foreach (var importDto in employees)
            {
                try
                {
                    // Find location by name
                    Location location = null;
                    if (!string.IsNullOrWhiteSpace(importDto.LocationName))
                    {
                        location = locations.FirstOrDefault(l =>
                            l.Name.Equals(importDto.LocationName, StringComparison.OrdinalIgnoreCase));

                        if (location == null)
                        {
                            // Create new location if not found
                            location = new Location(
                                GuidGenerator.Create(),
                                importDto.LocationName,
                                null,
                                CurrentTenant.Id
                            );
                            await _locationRepository.InsertAsync(location);
                            locations.Add(location);
                        }
                    }

                    // Create the employee
                    var createDto = new CreateEmployeeDto
                    {
                        FullName = importDto.FullName,
                        Email = importDto.Email,
                        Password = importDto.Password ?? "TempPassword123!",
                        PhoneNumber = importDto.PhoneNumber,
                        EmployeeCode = importDto.EmployeeCode,
                        LocationId = location?.Id,
                        IsActive = true
                    };

                    var created = await CreateAsync(createDto);
                    createdEmployees.Add(created);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Failed to import employee {importDto.Email}: {ex.Message}");
                    // Continue with next employee
                }
            }

            return createdEmployees;
        }

        public async Task<EmployeeDto> GetByUserIdAsync(Guid userId)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                throw new UserFriendlyException("Employee not found for this user");
            }

            return await GetAsync(employee.Id);
        }

        public async Task<List<EmployeeDto>> GetEmployeesByLocationAsync(Guid locationId)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Default);

            var queryable = await _employeeRepository.GetQueryableAsync();
            var employees = await AsyncExecuter.ToListAsync(
                queryable
                    .Include(e => e.Location)
                    .Where(e => e.LocationId == locationId && e.IsActive)
            );

            return ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);
        }

        public async Task<List<EmployeeLocationDto>> GetEmployeeLocationsAsync(Guid employeeId)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Default);

            var queryable = await _employeeLocationRepository.GetQueryableAsync();
            var employeeLocations = await AsyncExecuter.ToListAsync(
                queryable
                    .Include(el => el.Location)
                    .Where(el => el.EmployeeId == employeeId)
                    .OrderBy(el => el.Location.Name)
            );

            return employeeLocations.Select(el => new EmployeeLocationDto
            {
                Id = el.Id,
                EmployeeId = el.EmployeeId,
                LocationId = el.LocationId,
                LocationName = el.Location?.Name,
                CreationTime = el.CreationTime
            }).ToList();
        }

        public async Task AssignLocationsAsync(Guid employeeId, AssignLocationsDto input)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Edit);

            var employee = await _employeeRepository.GetAsync(employeeId);

            // Remove existing location assignments
            var existingLocations = await _employeeLocationRepository.GetListAsync(el => el.EmployeeId == employeeId);
            foreach (var location in existingLocations)
            {
                await _employeeLocationRepository.DeleteAsync(location);
            }

            // Add new location assignments
            foreach (var locationId in input.LocationIds)
            {
                var employeeLocation = new EmployeeLocation(
                    GuidGenerator.Create(),
                    employeeId,
                    locationId,
                    CurrentTenant.Id
                );

                await _employeeLocationRepository.InsertAsync(employeeLocation);
            }
        }

        public async Task RemoveLocationAsync(Guid employeeId, Guid locationId)
        {
            await CheckPolicyAsync(attendancePermissions.Employees.Edit);

            var employeeLocation = await _employeeLocationRepository.FirstOrDefaultAsync(
                el => el.EmployeeId == employeeId && el.LocationId == locationId);

            if (employeeLocation != null)
            {
                await _employeeLocationRepository.DeleteAsync(employeeLocation);
            }
        }

        private async Task CheckPolicyAsync(string policyName)
        {
            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
