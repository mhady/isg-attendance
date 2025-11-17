using System;
using System.Threading.Tasks;
using ISG.attendance.Permissions;
using Microsoft.Extensions.Logging;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace ISG.attendance.Data;

public class attendanceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly ILogger<attendanceDataSeedContributor> _logger;

    public attendanceDataSeedContributor(
        IIdentityRoleRepository roleRepository,
        IdentityRoleManager roleManager,
        IPermissionManager permissionManager,
        ILogger<attendanceDataSeedContributor> logger)
    {
        _roleRepository = roleRepository;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _logger = logger;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateRolesAsync();
        await AssignPermissionsAsync();
    }

    private async Task CreateRolesAsync()
    {
        // Create Company Admin role if it doesn't exist
        var companyAdminRole = await _roleRepository.FindByNormalizedNameAsync(
            _roleManager.NormalizeKey("CompanyAdmin"));

        if (companyAdminRole == null)
        {
            companyAdminRole = new IdentityRole(
                Guid.NewGuid(),
                "CompanyAdmin",
                tenantId: null) // This is a global role
            {
                IsPublic = true,
                IsDefault = false
            };

            await _roleManager.CreateAsync(companyAdminRole);
            _logger.LogInformation("Created CompanyAdmin role");
        }

        // Create Employee role if it doesn't exist
        var employeeRole = await _roleRepository.FindByNormalizedNameAsync(
            _roleManager.NormalizeKey("Employee"));

        if (employeeRole == null)
        {
            employeeRole = new IdentityRole(
                Guid.NewGuid(),
                "Employee",
                tenantId: null) // This is a global role
            {
                IsPublic = true,
                IsDefault = false
            };

            await _roleManager.CreateAsync(employeeRole);
            _logger.LogInformation("Created Employee role");
        }
    }

    private async Task AssignPermissionsAsync()
    {
        // Assign permissions to Company Admin role
        var companyAdminPermissions = new[]
        {
            // Employee Management - Full Access
            attendancePermissions.Employees.Default,
            attendancePermissions.Employees.Create,
            attendancePermissions.Employees.Edit,
            attendancePermissions.Employees.Delete,
            attendancePermissions.Employees.Import,

            // Location Management - Full Access
            attendancePermissions.Locations.Default,
            attendancePermissions.Locations.Create,
            attendancePermissions.Locations.Edit,
            attendancePermissions.Locations.Delete,

            // Attendance - View All
            attendancePermissions.Attendances.Default,
            attendancePermissions.Attendances.View,

            // Settings - Full Access
            attendancePermissions.Settings.Default,
            attendancePermissions.Settings.Manage,

            // Reports - Full Access
            attendancePermissions.Reports.Default,
            attendancePermissions.Reports.LocationSummary,
            attendancePermissions.Reports.MonthlyAttendance
        };

        foreach (var permission in companyAdminPermissions)
        {
            await _permissionManager.SetForRoleAsync(
                "CompanyAdmin",
                permission,
                true);
        }

        _logger.LogInformation("Assigned permissions to CompanyAdmin role");

        // Assign permissions to Employee role
        var employeePermissions = new[]
        {
            // Attendance - Only CheckIn/CheckOut and ViewOwn
            attendancePermissions.Attendances.Default,
            attendancePermissions.Attendances.CheckIn,
            attendancePermissions.Attendances.CheckOut,
            attendancePermissions.Attendances.ViewOwn
        };

        foreach (var permission in employeePermissions)
        {
            await _permissionManager.SetForRoleAsync(
                "Employee",
                permission,
                true);
        }

        _logger.LogInformation("Assigned permissions to Employee role");

        // Note: admin role already has all permissions by default in ABP
        // System admins can manage tenants (companies) through ABP's tenant management
    }
}
