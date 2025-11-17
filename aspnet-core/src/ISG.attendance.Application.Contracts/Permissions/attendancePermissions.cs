namespace ISG.attendance.Permissions;

public static class attendancePermissions
{
    public const string GroupName = "attendance";

    // Employee Management
    public static class Employees
    {
        public const string Default = GroupName + ".Employees";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Import = Default + ".Import";
    }

    // Location Management
    public static class Locations
    {
        public const string Default = GroupName + ".Locations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Attendance Management
    public static class Attendances
    {
        public const string Default = GroupName + ".Attendances";
        public const string View = Default + ".View";
        public const string CheckIn = Default + ".CheckIn";
        public const string CheckOut = Default + ".CheckOut";
        public const string ViewOwn = Default + ".ViewOwn";
    }

    // Company Settings
    public static class Settings
    {
        public const string Default = GroupName + ".Settings";
        public const string Manage = Default + ".Manage";
    }

    // Reports
    public static class Reports
    {
        public const string Default = GroupName + ".Reports";
        public const string LocationSummary = Default + ".LocationSummary";
        public const string MonthlyAttendance = Default + ".MonthlyAttendance";
    }
}
