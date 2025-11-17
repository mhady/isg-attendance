using ISG.attendance.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ISG.attendance.Permissions;

public class attendancePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var attendanceGroup = context.AddGroup(attendancePermissions.GroupName, L("Permission:Attendance"));

        // Employee Management Permissions
        var employeesPermission = attendanceGroup.AddPermission(attendancePermissions.Employees.Default, L("Permission:Employees"));
        employeesPermission.AddChild(attendancePermissions.Employees.Create, L("Permission:Employees.Create"));
        employeesPermission.AddChild(attendancePermissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeesPermission.AddChild(attendancePermissions.Employees.Delete, L("Permission:Employees.Delete"));
        employeesPermission.AddChild(attendancePermissions.Employees.Import, L("Permission:Employees.Import"));

        // Location Management Permissions
        var locationsPermission = attendanceGroup.AddPermission(attendancePermissions.Locations.Default, L("Permission:Locations"));
        locationsPermission.AddChild(attendancePermissions.Locations.Create, L("Permission:Locations.Create"));
        locationsPermission.AddChild(attendancePermissions.Locations.Edit, L("Permission:Locations.Edit"));
        locationsPermission.AddChild(attendancePermissions.Locations.Delete, L("Permission:Locations.Delete"));

        // Attendance Management Permissions
        var attendancesPermission = attendanceGroup.AddPermission(attendancePermissions.Attendances.Default, L("Permission:Attendances"));
        attendancesPermission.AddChild(attendancePermissions.Attendances.View, L("Permission:Attendances.View"));
        attendancesPermission.AddChild(attendancePermissions.Attendances.CheckIn, L("Permission:Attendances.CheckIn"));
        attendancesPermission.AddChild(attendancePermissions.Attendances.CheckOut, L("Permission:Attendances.CheckOut"));
        attendancesPermission.AddChild(attendancePermissions.Attendances.ViewOwn, L("Permission:Attendances.ViewOwn"));

        // Settings Permissions
        var settingsPermission = attendanceGroup.AddPermission(attendancePermissions.Settings.Default, L("Permission:Settings"));
        settingsPermission.AddChild(attendancePermissions.Settings.Manage, L("Permission:Settings.Manage"));

        // Reports Permissions
        var reportsPermission = attendanceGroup.AddPermission(attendancePermissions.Reports.Default, L("Permission:Reports"));
        reportsPermission.AddChild(attendancePermissions.Reports.LocationSummary, L("Permission:Reports.LocationSummary"));
        reportsPermission.AddChild(attendancePermissions.Reports.MonthlyAttendance, L("Permission:Reports.MonthlyAttendance"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<attendanceResource>(name);
    }
}
