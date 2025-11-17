using ISG.attendance.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ISG.attendance.Permissions;

public class attendancePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(attendancePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(attendancePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<attendanceResource>(name);
    }
}
